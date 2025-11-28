namespace Edumination.Api.Features.Attempts.Dtos;

public record SpeakingGradingResult
{
    public string TranscribedText { get; init; } = string.Empty;
    public int WordCount { get; init; }
    public decimal FluencyScore { get; init; }
    public decimal LexicalScore { get; init; }
    public decimal GrammarScore { get; init; }
    public decimal PronunciationScore { get; init; }
    public decimal OverallBand { get; init; }
    public string Feedback { get; init; } = string.Empty;
}
