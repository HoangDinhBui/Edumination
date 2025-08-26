using Edumination.Api.Features.Assets.Dtos;
using Edumination.Api.Domain.Entities;
using Education.Repositories.Interfaces;
using Edumination.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Education.Domain.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore; // Cần để sử dụng Include
namespace Edumination.Api.Services;

public class AssetsService : IAssetService
{
    private readonly IStorageService _storageService;
    private readonly IAssetRepository _assetRepository;
    private readonly IVirusScanner _virusScanner; // Thêm để quét virus

    public AssetsService(IStorageService storageService, IAssetRepository assetRepository, IVirusScanner virusScanner)
    {
        _storageService = storageService;
        _assetRepository = assetRepository;
        _virusScanner = virusScanner;
    }

    public async Task<CreateAssetResponseDto> CreateAssetAsync(CreateAssetDto dto, ClaimsPrincipal user)
    {
        if (!user.IsInRole("TEACHER") && !user.IsInRole("ADMIN"))
            throw new Exception("You are not allowed to upload assets.");

        var userIdStr = user.FindFirstValue(JwtRegisteredClaimNames.Sub)
                  ?? user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdStr) || !long.TryParse(userIdStr, out var userId))
            throw new Exception("Missing or Invalid user ID claim.");

        // Tạo đường dẫn lưu trữ
        string storageUrl = await _storageService.GenerateUploadPathAsync(dto.MediaType, dto.ByteSize);

        // Lưu vào DB (chưa có file thực tế, chỉ lưu metadata)
        var asset = new Asset
        {
            Kind = dto.Kind,
            MediaType = dto.MediaType,
            ByteSize = dto.ByteSize,
            StorageUrl = storageUrl,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow
        };

        var assetId = await _assetRepository.CreateAssetAsync(asset);

        // Trả về phản hồi
        return new CreateAssetResponseDto
        {
            UploadUrl = null,
            Asset = new AssetResponseDto { Id = assetId, StorageUrl = storageUrl }
        };
    }

    public async Task<AssetMetadataDto> GetAssetMetadataAndUrlAsync(long assetId, long userId, string[] userRoles)
    {
        // Bước 1: Lấy thông tin asset
        var asset = await _assetRepository.GetAssetByIdAsync(assetId);
        if (asset == null)
        {
            throw new Exception("Asset not found");
        }

        // Bước 2: Kiểm tra MIME type và kích thước
        var validMimeTypes = new[] { "video/mp4", "audio/mp3", "image/jpeg", "application/pdf" };
        if (!validMimeTypes.Contains(asset.MediaType))
        {
            throw new Exception("Invalid MIME type");
        }
        if (asset.ByteSize > 100 * 1024 * 1024) // Giới hạn 100MB
        {
            throw new Exception("File size exceeds limit");
        }

        // Bước 3: Quét virus (tùy chọn)
        if (bool.TryParse(Environment.GetEnvironmentVariable("VIRUS_SCAN_ENABLED") ?? "false", out bool virusScanEnabled) && virusScanEnabled)
        {
            var isSafe = await _virusScanner.ScanAsync(asset.StorageUrl);
            if (!isSafe)
            {
                throw new Exception("File contains malware");
            }
        }

        // Bước 4: Kiểm soát quyền đọc theo đối tượng tham chiếu
        if (asset.Kind == "AUDIO" || asset.Kind == "PDF")
        {
            var relatedPapers = await _assetRepository.GetRelatedTestPapersAsync(assetId);
            if (relatedPapers != null && relatedPapers.Any(tp => tp.Status == "DRAFT") && 
                !userRoles.Any(r => r == "TEACHER" || r == "ADMIN"))
            {
                throw new UnauthorizedAccessException("Access denied: Only TEACHER/ADMIN can view assets of DRAFT papers");
            }
        }

        // Bước 5: Tạo URL (vì không dùng S3, trả về đường dẫn cục bộ với kiểm soát truy cập qua server)
        var storageUrl = $"/api/v1/assets/download/{assetId}"; // URL giả định, cần endpoint riêng để phục vụ file

        // Trả về metadata
        return new AssetMetadataDto
        {
            Id = asset.Id,
            Kind = asset.Kind,
            MediaType = asset.MediaType,
            ByteSize = asset.ByteSize,
            StorageUrl = storageUrl,
            CreatedAt = asset.CreatedAt
        };
    }
}