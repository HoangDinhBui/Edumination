using Edumination.Api.Features.Assets.Dtos;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Edumination.Services.Interfaces;

public interface IAssetService
{
    Task<CreateAssetResponseDto> CreateAssetAsync(CreateAssetDto dto, ClaimsPrincipal user);
    Task<AssetMetadataDto> GetAssetMetadataAndUrlAsync(long assetId, long userId, string[] userRoles);
}