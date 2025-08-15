namespace Edumination.Api.Domain.Entities;

public class TestPaper
{
    public long Id { get; set; }
    public string? Code { get; set; }
    public string Title { get; set; } = default!;
    public string SourceType { get; set; } = "CUSTOM";
    public string UploadMethod { get; set; } = "MANUAL";
    public string Status { get; set; } = "DRAFT";
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
}
