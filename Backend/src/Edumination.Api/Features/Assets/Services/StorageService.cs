using System.IO;
using System.Threading.Tasks;
using Edumination.Services.Interfaces;
using Microsoft.Extensions.Configuration; // Giữ lại
using Microsoft.AspNetCore.Hosting; // <-- THÊM MỚI

namespace Edumination.Services;

public class StorageService : IStorageService
{
    private readonly string _baseStoragePath;

    // === BƯỚC 1: SỬA CONSTRUCTOR ===
    // Sửa IConfiguration thành IWebHostEnvironment
    public StorageService(IWebHostEnvironment env)
    {
        _baseStoragePath = Path.Combine(env.ContentRootPath, "uploads");

        // Tạo thư mục này nếu nó chưa tồn tại
        if (!Directory.Exists(_baseStoragePath))
        {
            Directory.CreateDirectory(_baseStoragePath);
        }
    }

    // === BƯỚC 2: HÀM NÀY GIỮ NGUYÊN ===
    public async Task<string> GenerateUploadPathAsync(string mediaType, long byteSize, CancellationToken ct = default)
    {
        // Tạo đường dẫn duy nhất dựa trên thời gian và GUID
        string fileExtension = GetFileExtension(mediaType);
        string fileName = $"{Guid.NewGuid()}{fileExtension}";
        string dateFolder = DateTime.UtcNow.ToString("yyyy-MM-dd");
        string relativePath = Path.Combine(dateFolder, fileName);

        // Trả về đường dẫn tương đối làm storage URL
        return $"/{relativePath}";
    }

    // === BƯỚC 3: HÀM NÀY GIỮ NGUYÊN ===
    // (Giờ nó sẽ lưu file vào /app/2025-11-12/...mp3)
    public async Task<string> SaveFileAsync(string filePath, Stream fileStream, CancellationToken ct = default)
    {
        string fullPath = Path.Combine(_baseStoragePath, filePath.TrimStart('/'));
        string? directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (var fileStreamLocal = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.CopyToAsync(fileStreamLocal, ct);
        }

        // Trả về đường dẫn tương đối
        return $"/{filePath.TrimStart('/')}";
    }

    // === BƯỚC 4: HÀM NÀY GIỮ NGUYÊN ===
    private string GetFileExtension(string mediaType)
    {
        return mediaType switch
        {
            "application/pdf" => ".pdf",
            "video/mp4" => ".mp4",
            "audio/mpeg" => ".mp3",
            "audio/wav" => ".wav",
            "audio/mp4" => ".m4a", // Thêm hỗ trợ m4a
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            _ => throw new InvalidOperationException("Unsupported media type.")
        };
    }

    // === BƯỚC 5: HÀM NÀY GIỮ NGUYÊN ===
    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default)
    {
        // Tích hợp GenerateUploadPathAsync và SaveFileAsync
        string filePath = await GenerateUploadPathAsync(contentType, stream.Length, ct);
        string storageUrl = await SaveFileAsync(filePath, stream, ct);
        return storageUrl;
    }
}