namespace Edumination.Api.Infrastructure.Options;

public class AuthOptions
{
    public int VerifyEmailTokenMinutes { get; set; } = 60; // hết hạn 60 phút
    public string JwtSecret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ResetPasswordTokenMinutes { get; set; } = 30;
}
