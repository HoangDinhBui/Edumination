namespace Edumination.Api.Domain.Entities;

public class PasswordReset
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string TokenHash { get; set; } = default!; // SHA-256 hex
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UsedAt { get; set; }

    public User User { get; set; } = default!;
}
