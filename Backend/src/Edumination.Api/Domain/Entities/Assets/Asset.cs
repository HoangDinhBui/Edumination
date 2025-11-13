using Edumination.Api.Domain.Entities;

namespace Education.Domain.Entities;

public class Asset
{
    public long Id { get; set; }
    public string Kind { get; set; } = string.Empty;
    public string? StorageUrl { get; set; } = string.Empty;
    public string? MediaType { get; set; } = string.Empty;
    public long ByteSize { get; set; }
    public int? DurationSec { get; set; } 
    public string? Sha256 { get; set; } 
    public string? LanguageCode { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<TestSection> TestSections { get; set; } = new List<TestSection>();
}