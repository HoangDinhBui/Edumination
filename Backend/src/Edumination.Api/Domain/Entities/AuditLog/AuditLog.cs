namespace Edumination.Api.Domain.Entities;

public class AuditLog
{
    public long Id { get; set; }
    public long? UserId { get; set; }    // có thể null khi chưa login (REGISTER)
    public string Action { get; set; } = default!;       // REGISTER, VERIFY_EMAIL, ...
    public string? EntityKind { get; set; }              // "USER"
    public long? EntityId { get; set; }
    public string? DataJson { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
