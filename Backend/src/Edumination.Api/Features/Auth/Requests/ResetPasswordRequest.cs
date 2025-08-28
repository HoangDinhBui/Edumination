namespace Edumination.Api.Features.Auth.Requests;

public class ResetPasswordRequest
{
    public string Token { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}