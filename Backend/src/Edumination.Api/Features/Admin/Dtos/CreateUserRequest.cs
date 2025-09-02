namespace Edumination.Api.Features.Admin.Dtos;

public class CreateUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "STUDENT"; // mặc định
    public bool Active { get; set; } = true;
}