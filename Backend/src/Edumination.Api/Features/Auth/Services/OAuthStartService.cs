using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Edumination.Api.Features.Auth.Services;

public interface IOAuthStartService
{
    Task<(string authUrl, string state)> StartAsync(string provider, string? returnUrl, CancellationToken ct);
}

public sealed class OAuthStartService : IOAuthStartService
{
    private readonly IMemoryCache _cache;
    private readonly OAuthOptions _opt;

    public OAuthStartService(IMemoryCache cache, IOptions<OAuthOptions> opt)
    {
        _cache = cache;
        _opt = opt.Value;
    }

    public Task<(string authUrl, string state)> StartAsync(string provider, string? returnUrl, CancellationToken ct)
    {
        provider = provider.ToLowerInvariant();
        return provider switch
        {
            "google" => Task.FromResult(BuildGoogleAuthUrl(_opt.Google, returnUrl)),
            _ => throw new NotSupportedException("Unsupported provider")
        };
    }

    private (string url, string state) BuildGoogleAuthUrl(OAuthProviderOptions p, string? returnUrl)
    {
        // 1) Tạo state + PKCE
        var state = Base64Url(RandomBytes(32));
        var codeVerifier = Base64Url(RandomBytes(32));
        var codeChallenge = CreateCodeChallenge(codeVerifier);

        // 2) Cache lại state -> {provider, verifier, returnUrl}
        var cacheItem = new OAuthStateCacheItem
        {
            Provider = "google",
            CodeVerifier = codeVerifier,
            ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? null : returnUrl
        };
        _cache.Set($"oauth:state:{state}", cacheItem, TimeSpan.FromMinutes(_opt.StateTtlMinutes));

        // 3) Ghép URL Google
        // https://developers.google.com/identity/openid-connect/openid-connect#authenticationuriparameters
        var query = new Dictionary<string, string?>
        {
            ["client_id"] = p.ClientId,
            ["redirect_uri"] = p.RedirectUri,
            ["response_type"] = "code",
            ["scope"] = string.Join(' ', p.Scopes.Length > 0 ? p.Scopes : new[] { "openid", "email", "profile" }),
            ["access_type"] = "offline",              // muốn có refresh_token
            ["include_granted_scopes"] = "true",
            ["prompt"] = "consent",                    // buộc consent để chắc refresh_token
            ["state"] = state,
            ["code_challenge"] = codeChallenge,
            ["code_challenge_method"] = "S256"
        };
        var url = QueryHelpers.AddQueryString("https://accounts.google.com/o/oauth2/v2/auth", query);
        return (url, state);
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
