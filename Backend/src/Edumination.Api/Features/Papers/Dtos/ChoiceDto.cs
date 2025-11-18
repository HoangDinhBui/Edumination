namespace Edumination.Api.Features.Papers.Dtos;

public record ChoiceDto
{

    public long Id { get; init; }
    public string? Content { get; init; }
    public bool IsCorrect { get; init; }
}