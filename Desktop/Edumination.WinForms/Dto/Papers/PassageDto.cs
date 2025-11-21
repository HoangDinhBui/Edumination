namespace Edumination.WinForms.Dto.Papers
{
    public record PassageDto
    {
        public long Id { get; init; }
        public string? Title { get; init; }
        public string? ContentText { get; init; }
        public List<QuestionDto>? Questions { get; init; }
    }
}

