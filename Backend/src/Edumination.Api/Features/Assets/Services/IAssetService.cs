using System.Security.Claims;
using Edumination.Api.Features.Assets.Dtos;

namespace Edumination.Services.Interfaces;

public interface IAssetService
{
    Task<CreateAssetResponseDto> CreateAssetAsync(CreateAssetDto dto, ClaimsPrincipal user);
}