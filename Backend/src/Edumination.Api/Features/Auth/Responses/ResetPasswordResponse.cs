namespace Edumination.Api.Features.Auth.Requests;

public class ResetPasswordResponse
{
    public long UserId { get; set; }
    public string Email { get; set; } = default!;
}