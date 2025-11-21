namespace Edumination.WinForms.Dto.Papers
{
    public record TestPaperDto
    {
        public long Id { get; init; }
        public string? Title { get; init; }
        public string? Status { get; init; }
        public DateTime CreatedAt { get; init; }
        public long? PdfAssetId { get; init; }
    }
}


