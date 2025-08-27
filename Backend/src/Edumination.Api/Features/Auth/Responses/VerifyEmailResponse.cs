namespace Edumination.Api.Features.Auth.Responses;

public class VerifyEmailResponse
{
    public long UserId { get; set; }
    public string Email { get; set; } = default!;
    public bool EmailVerified { get; set; }
}