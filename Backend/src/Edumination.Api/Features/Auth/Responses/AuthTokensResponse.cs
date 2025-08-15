namespace Edumination.Api.Features.Auth.Responses;

public record AuthTokensResponse(string AccessToken, string TokenType = "Bearer");
