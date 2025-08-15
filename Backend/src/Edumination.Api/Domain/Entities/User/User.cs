namespace Edumination.Api.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string Email { get; set; } = default!;
    public bool EmailVerified { get; set; }
    public string? PasswordHash { get; set; }
    public string FullName { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
