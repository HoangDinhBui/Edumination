namespace Edumination.WinForms.Dto.Papers
{
    public record ChoiceDto
    {
        public string? Content { get; init; }
        public bool IsCorrect { get; init; }
    }
}

