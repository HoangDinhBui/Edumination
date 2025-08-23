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
        _jwt = jwtOpt.Value; // ✅ gán trong constructor
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

        if (string.IsNullOrWhiteSpace(_jwt.Key) || _jwt.Key.Length < 32)
            throw new InvalidOperationException("JWT Key is missing or too short (>= 32 chars required).");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("name", user.FullName)
        };

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
}
