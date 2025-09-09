namespace Edumination.Api.Features.Papers.Dtos;

public record PassageDto
{
    public long Id { get; init; }
    public string? Title { get; init; }
    public string? ContentText { get; init; }
    public List<QuestionDto>? Questions { get; init; }
}