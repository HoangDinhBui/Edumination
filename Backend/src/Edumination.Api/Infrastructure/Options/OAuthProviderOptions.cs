public sealed class OAuthProviderOptions
{
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public string RedirectUri { get; set; } = "";
    public string[] Scopes { get; set; } = Array.Empty<string>();
}

public sealed class OAuthOptions
{
    public OAuthProviderOptions Google { get; set; } = new();
    public int StateTtlMinutes { get; set; } = 10;
}

public sealed class OAuthStateCacheItem
{
    public string Provider { get; set; } = "";
    public string CodeVerifier { get; set; } = "";
    public string? ReturnUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
