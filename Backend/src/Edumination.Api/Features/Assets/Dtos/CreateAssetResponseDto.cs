namespace Edumination.Api.Features.Assets.Dtos;

public class CreateAssetResponseDto
{
    public string? UploadUrl { get; set; } = string.Empty;
    public AssetResponseDto Asset { get; set; } = new();
}