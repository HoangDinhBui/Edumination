namespace Edumination.Api.Features.Admin.Dtos;

public sealed class AdminUserQuery
{
    public string? Email { get; set; }
    public string? Role { get; set; }         // ADMIN/TEACHER/STUDENT
    public bool? Active { get; set; }

    public int Page { get; set; } = 1;         // 1-based
    public int PageSize { get; set; } = 20;    // giới hạn hợp lý
}

public sealed class AdminUserItemDto
{
    public long Id { get; set; }
    public string Email { get; set; } = "";
    public string FullName { get; set; } = "";
    public bool IsActive { get; set; }
    public bool EmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public string[] Roles { get; set; } = Array.Empty<string>();
}

public sealed class PagedResult<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public long Total { get; set; }
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
}
