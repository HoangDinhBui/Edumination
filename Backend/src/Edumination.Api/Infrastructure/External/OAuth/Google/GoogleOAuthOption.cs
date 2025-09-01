using Microsoft.Extensions.Options;

public sealed class GoogleOAuthOptions
{
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public string RedirectUri { get; set; } = "";
}

public record GoogleTokenResponse(string access_token, string id_token, string? refresh_token, int expires_in, string token_type);
public record GoogleUserInfo(string sub, string email, bool email_verified, string name);

public interface IGoogleOAuthClient
{
    Task<GoogleTokenResponse> ExchangeCodeAsync(string code, string redirectUri, string codeVerifier, CancellationToken ct);
    Task<GoogleUserInfo> GetUserInfoAsync(string accessToken, CancellationToken ct);
}

public class GoogleOAuthClient : IGoogleOAuthClient
{
    private readonly HttpClient _http;
    private readonly GoogleOAuthOptions _opt;

    public GoogleOAuthClient(HttpClient http, IOptions<GoogleOAuthOptions> opt)
    {
        _http = http;
        _opt = opt.Value;
    }

    public async Task<GoogleTokenResponse> ExchangeCodeAsync(string code, string redirectUri, string? codeVerifier, CancellationToken ct)
    {
        var form = new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = _opt.ClientId,
            ["client_secret"] = _opt.ClientSecret,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code"
        };

        if (!string.IsNullOrEmpty(codeVerifier))
            form["code_verifier"] = codeVerifier;

        using var res = await _http.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(form), ct);
        var raw = await res.Content.ReadAsStringAsync(ct);
        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"Google token exchange failed {(int)res.StatusCode}: {raw}");

        return System.Text.Json.JsonSerializer.Deserialize<GoogleTokenResponse>(raw)!;
    }

    public async Task<GoogleUserInfo> GetUserInfoAsync(string accessToken, CancellationToken ct)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, "https://openidconnect.googleapis.com/v1/userinfo");
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        using var res = await _http.SendAsync(req, ct);
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync(ct);
        return System.Text.Json.JsonSerializer.Deserialize<GoogleUserInfo>(json)!;
    }
}
