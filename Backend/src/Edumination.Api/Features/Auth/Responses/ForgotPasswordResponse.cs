namespace Edumination.Api.Features.Auth.Responses
{
    public class ForgotPasswordResponse
    {
        // Trả về generic message để không lộ thông tin
        public string Message { get; set; } = "If an account with that email exists, a reset link has been sent.";
    }
}