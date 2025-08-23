namespace Edumination.Api.Infrastructure.Options;

public sealed class JwtOptions
{
    public string Key { get; set; } = string.Empty;     // >= 32 chars
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
