namespace Edumination.Services.Interfaces;

public interface IStorageService
{
    Task<string> GenerateUploadPathAsync(string mediaType, long byteSize, CancellationToken ct = default);
    Task<string> SaveFileAsync(string filePath, Stream fileStream, CancellationToken ct = default);
    // Tùy chọn: Thêm UploadAsync nếu muốn giữ logic trước đó
    Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default);
}