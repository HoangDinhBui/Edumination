namespace Edumination.Api.Features.Papers.Dtos;

public record SectionDto
{
    public long Id { get; init; }
    public string Skill { get; init; }
    public int SectionNo { get; init; }
    public List<PassageDto> Passages { get; init; }
}