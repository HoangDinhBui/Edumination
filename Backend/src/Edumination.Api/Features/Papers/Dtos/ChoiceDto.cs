namespace Edumination.Api.Features.Papers.Dtos;

public record ChoiceDto
{
    public string? Content { get; init; }
    public bool IsCorrect { get; init; }
}