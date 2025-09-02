namespace Edumination.Api.Features.Admin.Dtos;

public sealed class PatchUserRequest
{
    // Truyền field nào thì cập nhật field đó
    public string? FullName { get; set; }
    public string? Email { get; set; }          // tùy chọn: đổi email
    public string? Password { get; set; }       // nếu đổi password
    public bool? Active { get; set; }           // true/false
    public string? Role { get; set; }           // ví dụ: "ADMIN" | "TEACHER" | "STUDENT"
}

public sealed class AdminUserDto
{
    public long UserId { get; set; }
    public string Email { get; set; } = "";
    public string FullName { get; set; } = "";
    public bool IsActive { get; set; }
    public string[] Roles { get; set; } = Array.Empty<string>();
}
