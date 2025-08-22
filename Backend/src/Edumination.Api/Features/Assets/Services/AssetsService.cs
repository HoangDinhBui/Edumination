using Edumination.Api.Features.Assets.Dtos;
using Edumination.Api.Domain.Entities;
using Education.Repositories.Interfaces;
using Edumination.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Education.Domain.Entities;

namespace Edumination.Api.Services;

public class AssetsService : IAssetService
{
    private readonly IStorageService _storageService;
    private readonly IAssetRepository _assetRepository;

    public AssetsService(IStorageService storageService, IAssetRepository assetRepository)
    {
        _storageService = storageService;
        _assetRepository = assetRepository;
    }

    public async Task<CreateAssetResponseDto> CreateAssetAsync(CreateAssetDto dto, string userId)
    {
        // Tạo đường dẫn lưu trữ
        string storageUrl = await _storageService.GenerateUploadPathAsync(dto.MediaType, dto.ByteSize);

        // Lưu vào DB (chưa có file thực tế, chỉ lưu metadata)
        var asset = new Asset
        {
            Kind = dto.Kind,
            MediaType = dto.MediaType,
            ByteSize = dto.ByteSize,
            StorageUrl = storageUrl,
            CreatedBy = long.Parse(userId),
            CreatedAt = DateTime.UtcNow
        };

        var assetId = await _assetRepository.CreateAssetAsync(asset);

        // Trả về phản hồi (upload_url là null vì chưa upload trực tiếp)
        return new CreateAssetResponseDto
        {
            UploadUrl = null, // Hoặc để trống nếu không hỗ trợ presigned URL
            Asset = new AssetResponseDto { Id = assetId, StorageUrl = storageUrl }
        };
    }
}