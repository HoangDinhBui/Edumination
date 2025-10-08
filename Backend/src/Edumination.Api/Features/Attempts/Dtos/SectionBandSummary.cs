namespace Edumination.Api.Features.Attempts.Dtos;

public record SectionBandSummary
{
    public string Skill { get; set; } = string.Empty;
    public decimal? RawScore { get; set; }
    public decimal? BandScore { get; set; }
}