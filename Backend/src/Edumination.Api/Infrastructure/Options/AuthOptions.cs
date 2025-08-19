namespace Edumination.Api.Infrastructure.Options;

public class AuthOptions
{
    public int VerifyEmailTokenMinutes { get; set; } = 60; // hết hạn 60 phút
}
