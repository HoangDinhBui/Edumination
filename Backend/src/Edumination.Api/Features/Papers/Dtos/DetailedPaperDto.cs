namespace Edumination.Api.Features.Papers.Dtos;

public record DetailedPaperDto
{
    public long Id { get; init; }
    public string Title { get; init; }
    public string Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public long? PdfAssetId { get; init; }
    public List<SectionDto> Sections { get; init; }
}