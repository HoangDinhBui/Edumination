using Edumination.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace Education.Services;

public class StorageService : IStorageService
{
    private readonly string _baseStoragePath;

    public StorageService(IConfiguration configuration)
    {
        // Lấy đường dẫn lưu trữ từ cấu hình (ví dụ: appsettings.json)
        _baseStoragePath = configuration.GetValue<string>("Storage:BasePath") ?? "uploads";
        if (!Directory.Exists(_baseStoragePath))
        {
            Directory.CreateDirectory(_baseStoragePath);
        }
    }

    public async Task<string?> GenerateUploadPathAsync(string mediaType, long byteSize)
    {
        // Tạo đường dẫn duy nhất dựa trên thời gian và GUID
        string fileName = $"{Guid.NewGuid()}.{GetFileExtension(mediaType)}";
        string dateFolder = DateTime.UtcNow.ToString("yyyy-MM-dd");
        string fullPath = Path.Combine(_baseStoragePath, dateFolder, fileName);

        // Trả về đường dẫn tương đối làm storage URL
        return $"/{dateFolder}/{fileName}";
    }

    public async Task<string> SaveFileAsync(string filePath, Stream fileStream)
    {
        string fullPath = Path.Combine(_baseStoragePath, filePath.TrimStart('/'));
        string? directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (var fileStreamLocal = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.CopyToAsync(fileStreamLocal);
        }

        return fullPath;
    }

    private string GetFileExtension(string mediaType)
    {
        return mediaType switch
        {
            "application/pdf" => "pdf",
            "video/mp4" => "mp4",
            "audio/mpeg" => "mp3",
            "image/jpeg" => "jpg",
            "image/png" => "png",
            _ => "bin" // Mặc định nếu không nhận diện được
        };
    }
}