using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using Edumination.Api.Common.Results;
using Edumination.Api.Common.Services;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Features.Auth.Responses;
using Edumination.Api.Infrastructure.Options;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.Extensions.Caching.Memory;

namespace Edumination.Api.Features.Auth.Services
{
    //DTOs
    public sealed class OAuthStartDto
    {
        public string Provider { get; set; } = default!;
        public string AuthUrl { get; set; } = default!;
        public string State { get; set; } = default!;
        public int ExpiresInSeconds { get; set; }
    }

    public sealed class OAuthCallbackRequest
    {
        public string Code { get; set; } = default!;
        public string State { get; set; } = default!;
    }

    public sealed class LinkOAuthResponse
    {
        public string Provider { get; set; } = default!;
        public string Email { get; set; } = default!;
    }

    //Interface duy nhất
    public interface IOAuthService
    {
        // Bắt đầu flow OAuth cho login hoặc link (nếu userId != null)
        Task<(string authUrl, string state)> StartAsync(string provider, string? returnUrl, long? userId, CancellationToken ct);

        // Callback dành cho login (state không gắn userId)
        Task<AuthResponse> HandleCallbackAsync(string provider, OAuthCallbackRequest req, CancellationToken ct);

        // Link tài khoản mạng xã hội vào user đang đăng nhập
        Task<ApiResult<LinkOAuthResponse>> LinkAsync(long userId, string provider, OAuthCallbackRequest req, CancellationToken ct);

        // Gỡ liên kết
        Task<ApiResult<object>> UnlinkAsync(long userId, string provider, CancellationToken ct);
    }

    public sealed class OAuthService : IOAuthService
    {
        private readonly AppDbContext _db;
        private readonly IGoogleOAuthClient _google;
        private readonly IPasswordHasher _hasher;
        private readonly JwtOptions _jwt;
        private readonly OAuthOptions _oauthOpt;

        public OAuthService(
            AppDbContext db,
            IGoogleOAuthClient google,
            IPasswordHasher hasher,
            IOptions<JwtOptions> jwtOpt,
            IOptions<OAuthOptions> oauthOpt)
        {
            _db = db;
            _google = google;
            _hasher = hasher;
            _jwt = jwtOpt.Value;
            _oauthOpt = oauthOpt.Value;
        }

        // START (LOGIN / LINK)
        public async Task<(string authUrl, string state)> StartAsync(string provider, string? returnUrl, long? userId, CancellationToken ct)
        {
            provider = provider.ToLowerInvariant();
            if (provider != "google")
                throw new NotSupportedException("Provider not supported");

            var g = _oauthOpt.Google ?? throw new InvalidOperationException("Google OAuth options not configured.");
            var redirectUri = userId.HasValue ? g.RedirectUri : g.RedirectUriLogin;

            if (string.IsNullOrWhiteSpace(g.ClientId))
                throw new InvalidOperationException("Google OAuth: Missing ClientId configuration.");
            if (string.IsNullOrWhiteSpace(g.RedirectUri))
                throw new InvalidOperationException("Google OAuth: Missing RedirectUri configuration.");

            // 1) Generate state + PKCE
            var state = Base64Url(RandomBytes(32));
            var codeVerifier = Base64Url(RandomBytes(32));
            var codeChallenge = CreateCodeChallenge(codeVerifier);

            // 2) Persist state vào DB (kèm userId nếu flow là link)
            var expiresAt = DateTime.UtcNow.AddMinutes(_oauthOpt.StateTtlMinutes > 0 ? _oauthOpt.StateTtlMinutes : 10);

            _db.OAuthStates.Add(new OAuthStates
            {
                Provider = provider,
                State = state,
                ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? null : returnUrl,
                UserId = userId,
                ExpiresAt = expiresAt,
                CodeVerifier = codeVerifier
            });
            await _db.SaveChangesAsync(ct);

            // 3) Build Google Auth URL
            // Tham khảo: https://developers.google.com/identity/openid-connect/openid-connect#authenticationuriparameters
            var scopes = (g.Scopes is { Length: > 0 })
                ? string.Join(' ', g.Scopes)
                : "openid email profile";

            var query = new Dictionary<string, string?>
            {
                ["client_id"] = g.ClientId,
                ["redirect_uri"] = redirectUri,
                ["response_type"] = "code",
                ["scope"] = scopes,
                ["access_type"] = "offline",
                ["include_granted_scopes"] = "true",
                ["prompt"] = "consent",
                ["state"] = state,
                ["code_challenge"] = codeChallenge,
                ["code_challenge_method"] = "S256"
            };

            var authUrl = QueryHelpers.AddQueryString("https://accounts.google.com/o/oauth2/v2/auth", query);
            return (authUrl, state);
        }

        // CALLBACK LOGIN
        public async Task<AuthResponse> HandleCallbackAsync(string provider, OAuthCallbackRequest req, CancellationToken ct)
        {
            provider = provider.ToLowerInvariant();
            if (provider != "google")
                throw new NotSupportedException("Provider not supported");

            // Validate state
            var st = await _db.OAuthStates.SingleOrDefaultAsync(x => x.Provider == provider && x.State == req.State, ct);
            if (st == null || st.ExpiresAt < DateTime.UtcNow || st.ConsumedAt != null)
                throw new InvalidOperationException("Invalid or expired state");

            st.ConsumedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);

            // Đổi code -> token -> userinfo (ưu tiên dùng PKCE)
            // Khuyến nghị: IGoogleOAuthClient nên có overload ExchangeCodeAsync(code, codeVerifier, ct)
            var redirectUri = _oauthOpt.Google.RedirectUriLogin;
            var token = await _google.ExchangeCodeAsync(req.Code, redirectUri, st.CodeVerifier, ct);
            var info = await _google.GetUserInfoAsync(token.access_token, ct);

            // Tìm OAuthAccount
            var existing = await _db.OAuthAccounts
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Provider == provider && x.ProviderUserId == info.sub, ct);

            User user;
            if (existing != null)
            {
                existing.AccessToken = token.access_token;
                existing.RefreshToken = token.refresh_token ?? existing.RefreshToken;
                existing.AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(token.expires_in);
                existing.Email = info.email;
                existing.UpdatedAt = DateTime.UtcNow;
                user = existing.User;
            }
            else
            {
                // Map theo email nếu đã có, không thì tạo user mới (password null => đăng nhập bằng OAuth)
                user = await _db.Users.SingleOrDefaultAsync(u => u.Email == info.email, ct)
                       ?? new User
                       {
                           Email = info.email.ToLowerInvariant(),
                           EmailVerified = info.email_verified,
                           PasswordHash = "",
                           FullName = info.name ?? info.email,
                           IsActive = true
                       };

                if (user.Id == 0) _db.Users.Add(user);
                await _db.SaveChangesAsync(ct);

                _db.OAuthAccounts.Add(new OAuthAccounts
                {
                    UserId = user.Id,
                    Provider = provider,
                    ProviderUserId = info.sub,
                    Email = info.email,
                    AccessToken = token.access_token,
                    RefreshToken = token.refresh_token,
                    AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(token.expires_in)
                });
            }

            await _db.SaveChangesAsync(ct);

            // Phát JWT
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("name", user.FullName)
            };

            var roles = await _db.UserRoles.Where(r => r.UserId == user.Id)
                .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Code)
                .ToListAsync(ct);

            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };
        }

        // LINK OAUTH
        public async Task<ApiResult<LinkOAuthResponse>> LinkAsync(long userId, string provider, OAuthCallbackRequest req, CancellationToken ct)
        {
            provider = provider.ToLowerInvariant();
            if (provider != "google")
                return new(false, null, "Provider not supported");

            // State phải thuộc userId này
            var st = await _db.OAuthStates.SingleOrDefaultAsync(x => x.Provider == provider && x.State == req.State, ct);
            if (st == null || st.ExpiresAt < DateTime.UtcNow || st.ConsumedAt != null || st.UserId != userId)
                return new(false, null, "Invalid state");

            st.ConsumedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);

            var redirectUri = _oauthOpt.Google.RedirectUriLogin;
            var token = await _google.ExchangeCodeAsync(req.Code, redirectUri, st.CodeVerifier, ct);
            var info = await _google.GetUserInfoAsync(token.access_token, ct);

            // Không cho link nếu tài khoản social đã thuộc người khác
            var taken = await _db.OAuthAccounts.SingleOrDefaultAsync(x => x.Provider == provider && x.ProviderUserId == info.sub, ct);
            if (taken != null && taken.UserId != userId)
                return new(false, null, "This social account is already linked to another user.");

            // Nếu đã có record của user này -> update
            var record = await _db.OAuthAccounts.SingleOrDefaultAsync(x => x.Provider == provider && x.UserId == userId, ct);
            if (record == null)
            {
                record = new OAuthAccounts
                {
                    UserId = userId,
                    Provider = provider,
                    ProviderUserId = info.sub,
                    Email = info.email
                };
                _db.OAuthAccounts.Add(record);
            }

            record.AccessToken = token.access_token;
            record.RefreshToken = token.refresh_token ?? record.RefreshToken;
            record.AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(token.expires_in);
            record.Email = info.email;
            record.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);

            return new(true, new LinkOAuthResponse { Provider = provider, Email = info.email }, null);
        }

        // UNLINK OAUTH
        public async Task<ApiResult<object>> UnlinkAsync(long userId, string provider, CancellationToken ct)
        {
            provider = provider.ToLowerInvariant();
            if (provider != "google")
                return new(false, null, "Provider not supported");

            var record = await _db.OAuthAccounts.SingleOrDefaultAsync(x => x.Provider == provider && x.UserId == userId, ct);
            if (record == null)
                return new(true, new { }, null); // idempotent

            // An toàn: không gỡ nếu đây là phương thức đăng nhập duy nhất
            var user = await _db.Users.SingleAsync(x => x.Id == userId, ct);
            var otherProviders = await _db.OAuthAccounts.CountAsync(x => x.UserId == userId && x.Provider != provider, ct);
            var hasPassword = !string.IsNullOrEmpty(user.PasswordHash);

            if (!hasPassword && otherProviders == 0)
                return new(false, null, "Cannot unlink the last login method.");

            _db.OAuthAccounts.Remove(record);
            await _db.SaveChangesAsync(ct);

            return new(true, new { }, null);
        }

        // Helpers
        private static byte[] RandomBytes(int len)
        {
            var b = new byte[len];
            RandomNumberGenerator.Fill(b);
            return b;
        }

        private static string Base64Url(byte[] bytes) =>
            Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

        private static string CreateCodeChallenge(string verifier)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(verifier));
            return Base64Url(hash);
        }
    }
}
