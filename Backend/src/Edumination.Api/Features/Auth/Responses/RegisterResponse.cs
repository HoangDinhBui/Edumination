namespace Edumination.Api.Features.Auth.Responses;

public class RegisterResponse
{
    public long UserId { get; set; }
    public string Email { get; set; } = default!;
    public bool EmailVerified { get; set; }
    public string FullName { get; set; } = default!;
    public string VerifyEmailToken { get; set; } = default!; // trả để test/dev; production chỉ gửi mail
}
