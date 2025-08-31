using Edumination.Api.Domain.Entities;

namespace Edumination.Domain.Entities;

public class Question
{
    public long Id { get; set; }
    public long PassageId { get; set; }
    public string Qtype { get; set; } = "MCQ";
    public string Stem { get; set; } = string.Empty;
    public string? MetaJson { get; set; }
    public int Position { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public long? ExerciseId { get; set; }

    public virtual Passage? Passage { get; set; }
    public virtual Exercise? Exercise { get; set; }
    public virtual TestSection? Section { get; set; }
    public virtual ICollection<QuestionChoice>? QuestionChoices { get; set; }
    public virtual QuestionAnswerKey? QuestionAnswerKey { get; set; }
}