namespace Edumination.Api.Domain.Entities;

public class Answer
{
    public long Id { get; set; }
    public long SectionAttemptId { get; set; }
    public long QuestionId { get; set; }
    public string AnswerJson { get; set; }
    public bool? IsCorrect { get; set; }
    public decimal? EarnedScore { get; set; }
    public DateTime? CheckedAt { get; set; }

    // Quan hệ với SectionAttempt
    public SectionAttempt SectionAttempt { get; set; }
    // Quan hệ với Question
    public Question Question { get; set; }
}