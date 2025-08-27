namespace Edumination.Api.Features.Auth.Requests;

public class VerifyEmailRequest
{
    // Cho phép nhận từ body JSON: { "token": "..." }
    public string Token { get; set; } = default!;
}