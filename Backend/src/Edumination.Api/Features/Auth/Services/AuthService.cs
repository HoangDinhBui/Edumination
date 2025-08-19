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

namespace Edumination.Api.Features.Auth.Services;

public interface IAuthService
{
    Task<ApiResult<RegisterResponse>> RegisterAsync(RegisterRequest req, CancellationToken ct);
}

public class AuthService(
    AppDbContext db,
    IPasswordHasher hasher,
    IEmailSender emailSender,
    IAuditLogger audit,
    IOptions<AuthOptions> authOpt) : IAuthService
{
    public async Task<ApiResult<RegisterResponse>> RegisterAsync(RegisterRequest req, CancellationToken ct)
    {
        // 1) Check trùng email
        var emailNorm = req.Email.Trim().ToLowerInvariant();
        var exists = await db.Users.AnyAsync(u => u.Email == emailNorm, ct);
        if (exists)
            return new ApiResult<RegisterResponse>(false, null, "Email already registered.");

        // 2) Tạo user + role STUDENT
        var user = new User
        {
            Email = emailNorm,
            EmailVerified = false,
            PasswordHash = hasher.Hash(req.Password),
            FullName = req.Full_Name.Trim(),
            IsActive = true
        };
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        var studentRole = await db.Roles.FirstOrDefaultAsync(r => r.Code == "STUDENT", ct);
        if (studentRole != null)
        {
            db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = studentRole.Id });
            await db.SaveChangesAsync(ct);
        }

        // 3) Tạo token xác minh email (raw) + lưu hash
        var rawToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Guid.NewGuid().ToString("N");
        var tokenHash = rawToken.Sha256Hex();
        var ev = new EmailVerification
        {
            UserId = user.Id,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(authOpt.Value.VerifyEmailTokenMinutes)
        };
        db.EmailVerifications.Add(ev);
        await db.SaveChangesAsync(ct);

        // 4) Gửi email (link verify)
        var verifyUrl = $"https://your-frontend-domain/verify-email?token={Uri.EscapeDataString(rawToken)}";
        var html = $"""
            <h3>Verify your email</h3>
            <p>Hi {System.Net.WebUtility.HtmlEncode(user.FullName)},</p>
            <p>Please verify your email by clicking the link below:</p>
            <p><a href="{verifyUrl}">{verifyUrl}</a></p>
            <p>This link will expire in {authOpt.Value.VerifyEmailTokenMinutes} minutes.</p>
        """;
        await emailSender.SendAsync(user.Email, "Verify your email", html, ct);

        // 5) Audit
        await audit.LogAsync(user.Id, "REGISTER", "USER", user.Id, new { user.Email }, ct);

        // 6) Response (trả token chỉ cho DEV; production không nên trả)
        var resp = new RegisterResponse
        {
            UserId = user.Id,
            Email = user.Email,
            EmailVerified = false,
            FullName = user.FullName,
            VerifyEmailToken = rawToken
        };
        return new ApiResult<RegisterResponse>(true, resp, null);
    }
}
