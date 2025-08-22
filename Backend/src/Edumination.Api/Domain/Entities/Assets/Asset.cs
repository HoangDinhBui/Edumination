namespace Education.Domain.Entities;

public class Asset
{
    public long Id { get; set; }
    public string Kind { get; set; } = string.Empty;
    public string StorageUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public long ByteSize { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}