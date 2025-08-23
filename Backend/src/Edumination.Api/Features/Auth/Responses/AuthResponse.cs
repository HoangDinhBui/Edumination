namespace Edumination.Api.Features.Auth.Responses;

public class AuthResponse
{
    public string Token { get; set; } = default!;
    public long UserId { get; set; }
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
}
