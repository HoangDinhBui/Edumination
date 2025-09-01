namespace Edumination.Api.Domain.Entities;

public class OAuthStates
{
    public long Id { get; set; }
    public string Provider { get; set; } = default!;
    public string State { get; set; } = default!;
    public string? ReturnUrl { get; set; }
    public long? UserId { get; set; } // null = login; != null = link
    public DateTime ExpiresAt { get; set; }
    public DateTime? ConsumedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CodeVerifier { get; set; } = default!; // for PKCE
}
