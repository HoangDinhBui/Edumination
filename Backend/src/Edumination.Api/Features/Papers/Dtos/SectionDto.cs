namespace Edumination.Api.Features.Papers.Dtos;

public record SectionDto
{
    public long Id { get; set; }
    public string Skill { get; set; }
    public int SectionNo { get; set; }
    public int TimeLimitSec { get; set; }
    public long? AudioAssetId { get; set; }
    public bool IsPublished { get; set; }
    public List<PassageDto> Passages { get; set; }
}