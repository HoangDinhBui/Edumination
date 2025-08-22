namespace Edumination.Api.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string Email { get; set; } = default!;
    public bool EmailVerified { get; set; }
    public string? PasswordHash { get; set; }
    public string FullName { get; set; } = default!;
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

public class Role
{
    public long Id { get; set; }
    public string Code { get; set; } = default!; // STUDENT, TEACHER, ADMIN
    public string Name { get; set; } = default!;
}

public class UserRole
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = default!;
    public Role Role { get; set; } = default!;
}

public class UserEdu
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }
}