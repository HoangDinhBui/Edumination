namespace Edumination.Api.Features.Auth.Requests;

public class RegisterRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Full_Name { get; set; } = default!;
}
