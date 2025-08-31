namespace Edumination.Api.Features.Papers.Dtos;

public record QuestionDto
{
    public long Id { get; init; }
    public string Qtype { get; init; }
    public string Stem { get; init; }
    public int Position { get; init; }
    public List<ChoiceDto>? Choices { get; init; }  // Null nếu ẩn
    public object? AnswerKey { get; init; }  // JSON, null nếu ẩn
}