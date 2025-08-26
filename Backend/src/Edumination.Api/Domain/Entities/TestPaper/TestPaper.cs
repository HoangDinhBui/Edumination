using Education.Domain.Entities;

namespace Edumination.Api.Domain.Entities;

public class TestPaper
{
    public long Id { get; set; }
    public string? Code { get; set; }
    public string Title { get; set; } = default!;
    public string SourceType { get; set; } = "CUSTOM"; // Có thể thay bằng enum: OFFICIAL, CUSTOM
    public string UploadMethod { get; set; } = "MANUAL"; // Có thể thay bằng enum: PDF_PARSER, MANUAL
    public string Status { get; set; } = "DRAFT"; // Có thể thay bằng enum: DRAFT, REVIEW, PUBLISHED, ARCHIVED
    public long? PdfAssetId { get; set; } // Liên kết với Asset (PDF gốc)
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }

    // Navigation properties
    public virtual Asset? PdfAsset { get; set; } // Liên kết với Asset
    public virtual ICollection<TestSection> TestSections { get; set; } = new List<TestSection>();
}