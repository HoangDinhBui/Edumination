using System.Security.Claims;
using Edumination.Api.Common.Results;
using Edumination.Api.Features.Auth.Requests;
using Edumination.Api.Features.Auth.Responses;

namespace Edumination.Api.Features.Auth.Services;

public interface IAuthService
{
    Task<ApiResult<RegisterResponse>> RegisterAsync(RegisterRequest req, CancellationToken ct);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct);
    Task<ApiResult<VerifyEmailResponse>> VerifyEmailAsync(string rawToken, CancellationToken ct);
    Task<ApiResult<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest req, CancellationToken ct);
    Task<ApiResult<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest req, CancellationToken ct);
    Task<MeResponse> GetMeAsync(ClaimsPrincipal user, CancellationToken ct);
    Task<ApiResult<UpdateProfileResponse>> UpdateProfileAsync(long userId, UpdateProfileRequest req, CancellationToken ct);
}