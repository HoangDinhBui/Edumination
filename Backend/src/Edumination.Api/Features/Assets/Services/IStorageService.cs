namespace Edumination.Services.Interfaces;

public interface IStorageService
{
    Task<string> GenerateUploadPathAsync(string mediaType, long byteSize);
    Task<string> SaveFileAsync(string filePath, Stream fileStream);
}