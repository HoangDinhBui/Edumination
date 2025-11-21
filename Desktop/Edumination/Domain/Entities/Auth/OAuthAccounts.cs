namespace Edumination.Api.Domain.Entities;

public class OAuthAccounts
{
    public long Id { get; set; }
    public long UserId { get; set; }

    // "google" | "facebook" | ...
    public string Provider { get; set; } = default!;
    public string ProviderUserId { get; set; } = default!; // sub từ OpenID / id nhà cung cấp
    public string Email { get; set; } = default!;

    // Token tuỳ chọn (dùng lại nếu cần call API provider)
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? AccessTokenExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = default!;
}
