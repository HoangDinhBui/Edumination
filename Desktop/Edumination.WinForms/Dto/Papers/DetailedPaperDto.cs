

namespace Edumination.WinForms.Dto.Papers
{
    public record DetailedPaperDto
    {
        public long Id { get; init; }
        public string? Title { get; init; }
        public string? Status { get; init; }
        public DateTime CreatedAt { get; init; }
        //public string? PdfStorageUrl { get; set; }
        public long? PdfAssetId { get; init; }
        public List<SectionDto>? Sections { get; init; }
    }
}

