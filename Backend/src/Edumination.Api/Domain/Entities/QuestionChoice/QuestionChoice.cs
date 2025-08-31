namespace Edumination.Domain.Entities;

public class QuestionChoice
{
    internal object QuestionAnswerKey;

    public long Id { get; set; }
    public long QuestionId { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public int Position { get; set; }

    // Navigation property
    public virtual Question Question { get; set; }
}