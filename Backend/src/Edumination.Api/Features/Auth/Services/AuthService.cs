using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Edumination.Api.Common.Extensions;
using Edumination.Api.Common.Results;
using Edumination.Api.Common.Services;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Auth.Requests;
using Edumination.Api.Features.Auth.Responses;
using Edumination.Api.Infrastructure.External.Email;
using Edumination.Api.Infrastructure.Options;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Edumination.Api.Features.Auth.Services;

public interface IAuthService
{
    Task<ApiResult<RegisterResponse>> RegisterAsync(RegisterRequest req, CancellationToken ct);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct);
    Task<ApiResult<VerifyEmailResponse>> VerifyEmailAsync(string rawToken, CancellationToken ct);
    Task<ApiResult<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest req, CancellationToken ct);
    Task<ApiResult<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest req, CancellationToken ct);
    Task<MeResponse> GetMeAsync(ClaimsPrincipal user, CancellationToken ct);
    Task<ApiResult<UpdateProfileResponse>> UpdateProfileAsync(long userId, UpdateProfileRequest req, CancellationToken ct);
}

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IEmailSender _email;
    private readonly IAuditLogger _audit;
    private readonly AuthOptions _authOpt;
    private readonly AppOptions _appOpt;
    private readonly JwtOptions _jwt;

    public AuthService(
        AppDbContext db,
        IPasswordHasher hasher,
        IEmailSender emailSender,
        IAuditLogger audit,
        IOptions<AuthOptions> authOpt,
        IOptionsSnapshot<AppOptions> appOptions,
        IOptions<JwtOptions> jwtOpt)
    {
        _db = db;
        _hasher = hasher;
        _email = emailSender;
        _audit = audit;
        _authOpt = authOpt.Value;
        _appOpt = appOptions.Value;
        _jwt = jwtOpt.Value; //gán trong constructor
    }

    public async Task<ApiResult<RegisterResponse>> RegisterAsync(RegisterRequest req, CancellationToken ct)
    {
        var emailNorm = req.Email.Trim().ToLowerInvariant();
        if (await _db.Users.AnyAsync(u => u.Email == emailNorm, ct))
            return new ApiResult<RegisterResponse>(false, null, "Email already registered.");

        var user = new User
        {
            Email = emailNorm,
            EmailVerified = false,
            PasswordHash = _hasher.Hash(req.Password),
            FullName = req.Full_Name.Trim(),
            IsActive = true
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        var studentRole = await _db.Roles.FirstOrDefaultAsync(r => r.Code == "STUDENT", ct);
        if (studentRole != null)
        {
            _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = studentRole.Id });
            await _db.SaveChangesAsync(ct);
        }

        var rawToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Guid.NewGuid().ToString("N");
        var tokenHash = rawToken.Sha256Hex();
        _db.EmailVerifications.Add(new EmailVerification
        {
            UserId = user.Id,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_authOpt.VerifyEmailTokenMinutes)
        });
        await _db.SaveChangesAsync(ct);

        if (string.IsNullOrWhiteSpace(_appOpt.FrontendBaseUrl))
            throw new InvalidOperationException("Missing App:FrontendBaseUrl");

        var verifyUrl = $"{_appOpt.FrontendBaseUrl.TrimEnd('/')}/verify-email?token={Uri.EscapeDataString(rawToken)}";
        var html = $"""
            <h3>Verify your email</h3>
            <p>Hi {System.Net.WebUtility.HtmlEncode(user.FullName)},</p>
            <p>Please verify your email by clicking the link below:</p>
            <p><a href="{verifyUrl}">{verifyUrl}</a></p>
            <p>This link will expire in {_authOpt.VerifyEmailTokenMinutes} minutes.</p>
        """;
        await _email.SendAsync(user.Email, "Verify your email", html, ct);

        await _audit.LogAsync(user.Id, "REGISTER", "USER", user.Id, new { user.Email }, ct);

        return new ApiResult<RegisterResponse>(true, new RegisterResponse
        {
            UserId = user.Id,
            Email = user.Email,
            EmailVerified = false,
            FullName = user.FullName,
            VerifyEmailToken = rawToken // DEV only
        }, null);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await _db.Users.SingleOrDefaultAsync(x => x.Email == request.Email, ct);
        if (user == null || !_hasher.Verify(request.Password, user.PasswordHash!))
            throw new UnauthorizedAccessException("Invalid email or password");

        var role = await _db.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Code)
            .ToListAsync(ct);

        if (string.IsNullOrWhiteSpace(_jwt.Key) || _jwt.Key.Length < 32)
            throw new InvalidOperationException("JWT Key is missing or too short (>= 32 chars required).");

        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier,    user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("name", user.FullName),
             new("userid", user.Id.ToString())
        };

        foreach (var code in role)
            claims.Add(new Claim(ClaimTypes.Role, code));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName
        };
    }

    public async Task<ApiResult<VerifyEmailResponse>> VerifyEmailAsync(string rawToken, CancellationToken ct)
    {
        var normalized = NormalizeRawToken(rawToken);
        if (string.IsNullOrWhiteSpace(normalized))
            return new ApiResult<VerifyEmailResponse>(false, null, "Invalid token.");

        // Hash khớp với cách bạn đã lưu (Sha256Hex())
        var hash = normalized.Sha256Hex();

        // Lấy bản ghi token kèm user
        var ev = await _db.EmailVerifications
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.TokenHash == hash, ct);

        if (ev == null)
            return new ApiResult<VerifyEmailResponse>(false, null, "Token is invalid.");

        // Hết hạn?
        if (ev.ExpiresAt < DateTime.UtcNow)
            return new ApiResult<VerifyEmailResponse>(false, null, "Token is expired.");

        // Dùng rồi?
        if (ev.UsedAt != null)
        {
            // Idempotent: nếu user đã được verify thì có thể trả success “đã xác minh”
            if (ev.User.EmailVerified)
            {
                return new ApiResult<VerifyEmailResponse>(true, new VerifyEmailResponse
                {
                    UserId = ev.UserId,
                    Email = ev.User.Email,
                    EmailVerified = true
                }, null);
            }

            return new ApiResult<VerifyEmailResponse>(false, null, "Token already used.");
        }

        // Đánh dấu verified + dùng token
        ev.User.EmailVerified = true;
        ev.UsedAt = DateTime.UtcNow;

        // (khuyến nghị) Xoá các token cũ khác của user (nếu muốn single-use thật sự)
        var others = await _db.EmailVerifications
            .Where(x => x.UserId == ev.UserId && x.Id != ev.Id && x.UsedAt == null)
            .ToListAsync(ct);
        foreach (var other in others) other.UsedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        // Audit
        await _audit.LogAsync(ev.UserId, "VERIFY_EMAIL", "USER", ev.UserId, new { ev.User.Email }, ct);

        return new ApiResult<VerifyEmailResponse>(true, new VerifyEmailResponse
        {
            UserId = ev.UserId,
            Email = ev.User.Email,
            EmailVerified = true
        }, null);
    }

    private static string NormalizeRawToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return string.Empty;

        // nhiều client biến '+' thành space
        string t = token.Trim().Replace(' ', '+');

        // gỡ URL-encode tối đa 2 lần
        for (int i = 0; i < 2; i++)
        {
            if (t.Contains('%'))
            {
                t = Uri.UnescapeDataString(t);
                t = t.Replace(' ', '+');
            }
            else break;
        }

        return t;
    }

    public async Task<ApiResult<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest req, CancellationToken ct)
    {
        var emailNorm = (req.Email ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(emailNorm))
            return new(false, null, "Email is required");

        // Tìm user (nhưng luôn trả 200 dù có hay không)
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == emailNorm && u.IsActive, ct);

        // Luôn trả message “đã gửi” để tránh lộ tồn tại email
        var okResp = new ForgotPasswordResponse();

        if (user is null)
        {
            // Tùy chọn: sleep nhẹ để tránh timing attack
            await Task.Delay(200, ct);
            return new(true, okResp, null);
        }

        // Invalidate/reset các token cũ chưa dùng
        var oldTokens = await _db.PasswordResets
            .Where(x => x.UserId == user.Id && x.UsedAt == null && x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(ct);
        if (oldTokens.Count > 0)
        {
            _db.PasswordResets.RemoveRange(oldTokens);
            await _db.SaveChangesAsync(ct);
        }

        // Tạo raw token + hash
        var rawToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Guid.NewGuid().ToString("N");
        var tokenHash = rawToken.Sha256Hex();

        var pr = new PasswordReset
        {
            UserId = user.Id,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_authOpt.ResetPasswordTokenMinutes)
        };
        _db.PasswordResets.Add(pr);
        await _db.SaveChangesAsync(ct);

        // Tạo link reset (điểm đến FE)
        if (string.IsNullOrWhiteSpace(_appOpt.FrontendBaseUrl))
            throw new InvalidOperationException("Missing App:FrontendBaseUrl");

        var resetUrl = $"{_appOpt.FrontendBaseUrl.TrimEnd('/')}/reset-password?token={Uri.EscapeDataString(rawToken)}";

        var html = $"""
            <h3>Reset your password</h3>
            <p>Hi {System.Net.WebUtility.HtmlEncode(user.FullName)},</p>
            <p>You (or someone) requested to reset your password. Click the link below to proceed:</p>
            <p><a href="{resetUrl}">{resetUrl}</a></p>
            <p>This link will expire in {_authOpt.ResetPasswordTokenMinutes} minutes. If you didn’t request this, you can safely ignore this email.</p>
        """;

        await _email.SendAsync(user.Email, "Reset your password", html, ct);

        await _audit.LogAsync(user.Id, "PASSWORD_FORGOT_REQUEST", "USER", user.Id, new { user.Email }, ct);

        return new(true, okResp, null);
    }

    public async Task<ApiResult<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest req, CancellationToken ct)
    {
        var raw = NormalizeRawToken(req.Token);
        if (string.IsNullOrWhiteSpace(raw))
            return new(false, null, "Invalid token.");

        var tokenHash = raw.Sha256Hex();

        var pr = await _db.PasswordResets
            .Include(x => x.User)
            .OrderByDescending(x => x.Id)
            .SingleOrDefaultAsync(x => x.TokenHash == tokenHash, ct);

        if (pr == null) return new(false, null, "Token is invalid.");
        if (pr.ExpiresAt < DateTime.UtcNow) return new(false, null, "Token is expired.");
        if (pr.UsedAt != null) return new(false, null, "Token already used.");

        // TODO: validate password (độ dài, ký tự đặc biệt…)
        // if (!IsStrong(req.NewPassword)) return new(false, null, "Weak password.");

        pr.User.PasswordHash = _hasher.Hash(req.NewPassword);
        pr.UsedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        await _audit.LogAsync(pr.UserId, "PASSWORD_RESET", "USER", pr.UserId, new { pr.User.Email }, ct);

        return new(true, new ResetPasswordResponse { Email = pr.User.Email }, null);
    }

    public async Task<MeResponse> GetMeAsync(ClaimsPrincipal principal, CancellationToken ct)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? principal.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("Missing user id claim.");

        var id = long.Parse(userId);

        // Lấy user
        var u = await _db.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);

        if (u is null)
            throw new UnauthorizedAccessException("User not found or inactive.");

        // Lấy roles (Code: STUDENT/TEACHER/ADMIN)
        var roleCodes = await _db.UserRoles
            .Where(ur => ur.UserId == id)
            .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Code)
            .OrderBy(code => code)
            .ToArrayAsync(ct);

        return new MeResponse
        {
            Profile = new MeProfileDto
            {
                UserId = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                AvatarUrl = u.AvatarUrl,
                EmailVerified = u.EmailVerified
            },
            Roles = roleCodes
        };
    }

    public async Task<ApiResult<UpdateProfileResponse>> UpdateProfileAsync(long userId, UpdateProfileRequest req, CancellationToken ct)
    {
        // Lấy user hiện tại
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == userId && u.IsActive, ct);
        if (user == null)
            return new(false, null, "User not found or inactive.");

        // Chuẩn hoá input
        var fullName = (req.FullName ?? "").Trim();
        var avatar = string.IsNullOrWhiteSpace(req.AvatarUrl) ? null : req.AvatarUrl!.Trim();

        // Cập nhật
        bool changed = false;
        if (!string.IsNullOrWhiteSpace(fullName) && !string.Equals(user.FullName, fullName, StringComparison.Ordinal))
        {
            user.FullName = fullName;
            changed = true;
        }
        if (avatar != user.AvatarUrl)
        {
            user.AvatarUrl = avatar;
            changed = true;
        }

        if (!changed)
            return new(true, new UpdateProfileResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                EmailVerified = user.EmailVerified
            }, null);

        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        // Audit
        await _audit.LogAsync(user.Id, "UPDATE_PROFILE", "USER", user.Id, new { user.FullName, user.AvatarUrl }, ct);

        return new(true, new UpdateProfileResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            EmailVerified = user.EmailVerified
        }, null);
    }
}
