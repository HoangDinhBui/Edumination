using Edumination.Api.Common.Results;
using Edumination.Api.Features.Auth.Responses;

namespace Edumination.Api.Features.Auth.Services;

public interface IOAuthService
{
    // Bắt đầu flow OAuth cho login hoặc link (nếu userId != null)
    Task<(string authUrl, string state)> StartAsync(string provider, string? returnUrl, long? userId, CancellationToken ct);

    // Callback dành cho login (state không gắn userId)
    Task<AuthResponse> HandleCallbackAsync(string provider, OAuthCallbackRequest req, CancellationToken ct);

    // Link tài khoản mạng xã hội vào user đang đăng nhập
    Task<ApiResult<LinkOAuthResponse>> LinkAsync(long userId, string provider, OAuthCallbackRequest req, CancellationToken ct);

    // Gỡ liên kết
    Task<ApiResult<object>> UnlinkAsync(long userId, string provider, CancellationToken ct);
}