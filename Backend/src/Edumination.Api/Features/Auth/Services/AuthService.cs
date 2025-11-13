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
using Microsoft.Extensions.Logging;

namespace Edumination.Api.Features.Auth.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IEmailSender _email;
    private readonly IAuditLogger _audit;
    private readonly ILogger<AuthService> _logger;
    private readonly AuthOptions _authOpt;
    private readonly AppOptions _appOpt;
    private readonly JwtOptions _jwt;

    public AuthService(
        AppDbContext db,
        IPasswordHasher hasher,
        IEmailSender emailSender,
        IAuditLogger audit,
        ILogger<AuthService> logger,
        IOptions<AuthOptions> authOpt,
        IOptionsSnapshot<AppOptions> appOptions,
        IOptions<JwtOptions> jwtOpt)
    {
        _db = db;
        _hasher = hasher;
        _email = emailSender;
        _audit = audit;
        _logger = logger;
        _authOpt = authOpt.Value;
        _appOpt = appOptions.Value;
        _jwt = jwtOpt.Value;
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

        // Tạo token xác thực email
        var rawToken = GenerateSecureToken();
        var tokenHash = rawToken.Sha256Hex();
        _db.EmailVerifications.Add(new EmailVerification
        {
            UserId = user.Id,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_authOpt.VerifyEmailTokenMinutes)
        });
        await _db.SaveChangesAsync(ct);

        // Gửi email xác thực (không chặn flow nếu fail)
        try
        {
            await SendVerificationEmailAsync(user, rawToken, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send verification email to {Email}", user.Email);
            // Không throw - vẫn cho đăng ký thành công, user có thể resend sau
        }

        await _audit.LogAsync(user.Id, "REGISTER", "USER", user.Id, new { user.Email }, ct);

        return new ApiResult<RegisterResponse>(true, new RegisterResponse
        {
            UserId = user.Id,
            Email = user.Email,
            EmailVerified = false,
            FullName = user.FullName,
            // Không trả về token trong response vì lý do bảo mật
            // User sẽ nhận token qua email
        }, null);
    }

    private async Task SendVerificationEmailAsync(User user, string rawToken, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_appOpt.FrontendBaseUrl))
            throw new InvalidOperationException("Missing App:FrontendBaseUrl");

        var verifyUrl = $"{_appOpt.FrontendBaseUrl.TrimEnd('/')}/verify-email?token={Uri.EscapeDataString(rawToken)}";

        var html = BuildEmailTemplate(
            title: "Xác thực email của bạn",
            greeting: $"Chào {System.Net.WebUtility.HtmlEncode(user.FullName)},",
            content: $@"
                <p>Cảm ơn bạn đã đăng ký tài khoản Edumination!</p>
                <p>Vui lòng xác thực địa chỉ email của bạn bằng cách nhấp vào nút bên dưới:</p>
                <div style='text-align: center; margin: 30px 0;'>
                    <a href='{verifyUrl}' 
                       style='background-color: #4F46E5; color: white; padding: 12px 30px; 
                              text-decoration: none; border-radius: 6px; display: inline-block;
                              font-weight: 600;'>
                        Xác thực Email
                    </a>
                </div>
                <p style='color: #6B7280; font-size: 14px;'>
                    Hoặc copy link này vào trình duyệt:<br>
                    <a href='{verifyUrl}' style='color: #4F46E5; word-break: break-all;'>{verifyUrl}</a>
                </p>
                <p style='color: #6B7280; font-size: 14px;'>
                    Link này sẽ hết hạn sau {_authOpt.VerifyEmailTokenMinutes} phút.
                </p>
            ",
            footer: "Nếu bạn không đăng ký tài khoản này, vui lòng bỏ qua email này."
        );

        await _email.SendAsync(user.Email, "Xác thực email - Edumination", html, ct);
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

        var hash = normalized.Sha256Hex();

        var ev = await _db.EmailVerifications
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.TokenHash == hash, ct);

        if (ev == null)
            return new ApiResult<VerifyEmailResponse>(false, null, "Token không hợp lệ.");

        if (ev.ExpiresAt < DateTime.UtcNow)
            return new ApiResult<VerifyEmailResponse>(false, null, "Token đã hết hạn. Vui lòng yêu cầu gửi lại email xác thực.");

        if (ev.UsedAt != null)
        {
            // Idempotent: nếu user đã được verify thì trả success
            if (ev.User.EmailVerified)
            {
                return new ApiResult<VerifyEmailResponse>(true, new VerifyEmailResponse
                {
                    UserId = ev.UserId,
                    Email = ev.User.Email,
                    EmailVerified = true
                }, null);
            }

            return new ApiResult<VerifyEmailResponse>(false, null, "Token đã được sử dụng.");
        }

        // Đánh dấu verified
        ev.User.EmailVerified = true;
        ev.UsedAt = DateTime.UtcNow;

        // Vô hiệu hóa các token cũ khác
        var others = await _db.EmailVerifications
            .Where(x => x.UserId == ev.UserId && x.Id != ev.Id && x.UsedAt == null)
            .ToListAsync(ct);
        foreach (var other in others)
            other.UsedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        await _audit.LogAsync(ev.UserId, "VERIFY_EMAIL", "USER", ev.UserId, new { ev.User.Email }, ct);

        return new ApiResult<VerifyEmailResponse>(true, new VerifyEmailResponse
        {
            UserId = ev.UserId,
            Email = ev.User.Email,
            EmailVerified = true
        }, null);
    }

    public async Task<ApiResult<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest req, CancellationToken ct)
    {
        var emailNorm = (req.Email ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(emailNorm))
            return new(false, null, "Email không được để trống");

        // Luôn trả về success để tránh lộ thông tin user tồn tại
        var okResp = new ForgotPasswordResponse();

        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == emailNorm && u.IsActive, ct);

        if (user is null)
        {
            // Fake delay để tránh timing attack
            await Task.Delay(Random.Shared.Next(100, 300), ct);
            _logger.LogWarning("Password reset requested for non-existent email: {Email}", emailNorm);
            return new(true, okResp, null);
        }

        // Vô hiệu hóa các token reset cũ chưa dùng
        var oldTokens = await _db.PasswordResets
            .Where(x => x.UserId == user.Id && x.UsedAt == null && x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(ct);

        if (oldTokens.Count > 0)
        {
            _db.PasswordResets.RemoveRange(oldTokens);
            await _db.SaveChangesAsync(ct);
        }

        // Tạo token mới
        var rawToken = GenerateSecureToken();
        var tokenHash = rawToken.Sha256Hex();

        var pr = new PasswordReset
        {
            UserId = user.Id,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_authOpt.ResetPasswordTokenMinutes)
        };
        _db.PasswordResets.Add(pr);
        await _db.SaveChangesAsync(ct);

        // Gửi email (không chặn flow nếu fail)
        try
        {
            await SendResetPasswordEmailAsync(user, rawToken, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", user.Email);
            // Vẫn trả về success để không lộ lỗi
        }

        await _audit.LogAsync(user.Id, "PASSWORD_FORGOT_REQUEST", "USER", user.Id, new { user.Email }, ct);

        return new(true, okResp, null);
    }

    private async Task SendResetPasswordEmailAsync(User user, string rawToken, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_appOpt.FrontendBaseUrl))
            throw new InvalidOperationException("Missing App:FrontendBaseUrl");

        var resetUrl = $"{_appOpt.FrontendBaseUrl.TrimEnd('/')}/reset-password?token={Uri.EscapeDataString(rawToken)}";

        var html = BuildEmailTemplate(
            title: "Đặt lại mật khẩu",
            greeting: $"Chào {System.Net.WebUtility.HtmlEncode(user.FullName)},",
            content: $@"
                <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
                <p>Nhấp vào nút bên dưới để tạo mật khẩu mới:</p>
                <div style='text-align: center; margin: 30px 0;'>
                    <a href='{resetUrl}' 
                       style='background-color: #DC2626; color: white; padding: 12px 30px; 
                              text-decoration: none; border-radius: 6px; display: inline-block;
                              font-weight: 600;'>
                        Đặt lại mật khẩu
                    </a>
                </div>
                <p style='color: #6B7280; font-size: 14px;'>
                    Hoặc copy link này vào trình duyệt:<br>
                    <a href='{resetUrl}' style='color: #DC2626; word-break: break-all;'>{resetUrl}</a>
                </p>
                <p style='color: #6B7280; font-size: 14px;'>
                    Link này sẽ hết hạn sau {_authOpt.ResetPasswordTokenMinutes} phút.
                </p>
                <p style='background-color: #FEF3C7; padding: 12px; border-radius: 6px; 
                          border-left: 4px solid #F59E0B; color: #92400E; font-size: 14px;'>
                    <strong>⚠️ Lưu ý bảo mật:</strong> Nếu bạn không yêu cầu đặt lại mật khẩu, 
                    vui lòng bỏ qua email này và kiểm tra bảo mật tài khoản của bạn.
                </p>
            ",
            footer: "Email này được gửi tự động, vui lòng không trả lời."
        );

        await _email.SendAsync(user.Email, "Đặt lại mật khẩu - Edumination", html, ct);
    }

    public async Task<ApiResult<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest req, CancellationToken ct)
    {
        var raw = NormalizeRawToken(req.Token);
        if (string.IsNullOrWhiteSpace(raw))
            return new(false, null, "Token không hợp lệ.");

        var tokenHash = raw.Sha256Hex();

        var pr = await _db.PasswordResets
            .Include(x => x.User)
            .OrderByDescending(x => x.Id)
            .SingleOrDefaultAsync(x => x.TokenHash == tokenHash, ct);

        if (pr == null)
            return new(false, null, "Token không hợp lệ.");

        if (pr.ExpiresAt < DateTime.UtcNow)
            return new(false, null, "Token đã hết hạn. Vui lòng yêu cầu đặt lại mật khẩu mới.");

        if (pr.UsedAt != null)
            return new(false, null, "Token đã được sử dụng.");

        // Validate password strength
        if (string.IsNullOrWhiteSpace(req.NewPassword) || req.NewPassword.Length < 8)
            return new(false, null, "Mật khẩu phải có ít nhất 8 ký tự.");

        // Đổi mật khẩu
        pr.User.PasswordHash = _hasher.Hash(req.NewPassword);
        pr.UsedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        await _audit.LogAsync(pr.UserId, "PASSWORD_RESET", "USER", pr.UserId, new { pr.User.Email }, ct);

        // Gửi email thông báo đổi mật khẩu thành công (optional nhưng nên có)
        try
        {
            await SendPasswordChangedNotificationAsync(pr.User, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password changed notification to {Email}", pr.User.Email);
        }

        return new(true, new ResetPasswordResponse { Email = pr.User.Email }, null);
    }

    private async Task SendPasswordChangedNotificationAsync(User user, CancellationToken ct)
    {
        var html = BuildEmailTemplate(
            title: "Mật khẩu đã được thay đổi",
            greeting: $"Chào {System.Net.WebUtility.HtmlEncode(user.FullName)},",
            content: @"
                <p>Mật khẩu tài khoản của bạn đã được thay đổi thành công.</p>
                <p>Thời gian: <strong>" + DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss") + @" UTC</strong></p>
                <p style='background-color: #FEF3C7; padding: 12px; border-radius: 6px; 
                          border-left: 4px solid #F59E0B; color: #92400E; font-size: 14px;'>
                    <strong>⚠️ Cảnh báo:</strong> Nếu bạn không thực hiện thay đổi này, 
                    vui lòng liên hệ ngay với chúng tôi để bảo vệ tài khoản.
                </p>
            ",
            footer: "Đây là email thông báo bảo mật."
        );

        await _email.SendAsync(user.Email, "Mật khẩu đã được thay đổi - Edumination", html, ct);
    }

    public async Task<MeResponse> GetMeAsync(ClaimsPrincipal principal, CancellationToken ct)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("Missing user id claim.");

        var id = long.Parse(userId);

        var u = await _db.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);

        if (u is null)
            throw new UnauthorizedAccessException("User not found or inactive.");

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
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == userId && u.IsActive, ct);
        if (user == null)
            return new(false, null, "User not found or inactive.");

        var fullName = (req.FullName ?? "").Trim();
        var avatar = string.IsNullOrWhiteSpace(req.AvatarUrl) ? null : req.AvatarUrl!.Trim();

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

        await _audit.LogAsync(user.Id, "UPDATE_PROFILE", "USER", user.Id,
            new { user.FullName, user.AvatarUrl }, ct);

        return new(true, new UpdateProfileResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            EmailVerified = user.EmailVerified
        }, null);
    }

    // ============ HELPER METHODS ============

    private static string NormalizeRawToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return string.Empty;

        string t = token.Trim().Replace(' ', '+');

        // Decode URL encoding (tối đa 2 lần)
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

    private static string GenerateSecureToken()
    {
        // Tạo token an toàn hơn: 32 bytes random + timestamp
        var randomBytes = new byte[32];
        System.Security.Cryptography.RandomNumberGenerator.Fill(randomBytes);
        var timestamp = DateTime.UtcNow.Ticks;
        return Convert.ToBase64String(randomBytes) + timestamp.ToString("X");
    }

    private static string BuildEmailTemplate(string title, string greeting, string content, string footer)
    {
        return $@"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{title}</title>
</head>
<body style='margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, ""Helvetica Neue"", Arial, sans-serif; background-color: #F3F4F6;'>
    <table role='presentation' style='width: 100%; border-collapse: collapse;'>
        <tr>
            <td align='center' style='padding: 40px 0;'>
                <table role='presentation' style='width: 600px; max-width: 100%; border-collapse: collapse; background-color: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
                    <!-- Header -->
                    <tr>
                        <td style='padding: 40px 40px 20px; text-align: center; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 8px 8px 0 0;'>
                            <h1 style='margin: 0; color: white; font-size: 28px; font-weight: 700;'>
                                Edumination
                            </h1>
                        </td>
                    </tr>
                    
                    <!-- Content -->
                    <tr>
                        <td style='padding: 40px;'>
                            <h2 style='margin: 0 0 20px; color: #111827; font-size: 24px; font-weight: 600;'>
                                {title}
                            </h2>
                            <p style='margin: 0 0 15px; color: #374151; font-size: 16px; line-height: 1.6;'>
                                {greeting}
                            </p>
                            <div style='color: #374151; font-size: 16px; line-height: 1.6;'>
                                {content}
                            </div>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style='padding: 30px 40px; background-color: #F9FAFB; border-radius: 0 0 8px 8px; text-align: center;'>
                            <p style='margin: 0 0 10px; color: #6B7280; font-size: 14px;'>
                                {footer}
                            </p>
                            <p style='margin: 0; color: #9CA3AF; font-size: 12px;'>
                                © {DateTime.UtcNow.Year} Edumination. All rights reserved.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }
}