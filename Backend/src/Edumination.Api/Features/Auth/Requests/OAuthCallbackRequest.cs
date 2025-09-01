namespace Edumination.Api.Features.Auth.Requests;

public sealed class OAuthCallbackRequest
{
    public string Code { get; set; } = "";
    public string State { get; set; } = "";
    public string? ReturnUrl { get; set; }
}
