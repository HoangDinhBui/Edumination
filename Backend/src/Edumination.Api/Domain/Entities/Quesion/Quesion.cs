using System;

namespace Edumination.Domain.Entities;

public class Question
{
    public long Id { get; set; }
    public long PassageId { get; set; } // Liên kết với Passage
    public string Qtype { get; set; } = "MCQ"; // ENUM: MCQ, MULTI_SELECT, FILL_BLANK, MATCHING, ORDERING, SHORT_ANSWER, ESSAY, SPEAK_PROMPT
    public string Stem { get; set; } = default!;
    public string? MetaJson { get; set; } // JSON chứa điểm, hint, rubric nhỏ, số blank...
    public int Position { get; set; }
    public DateTime CreatedAt { get; set; }
    public long? ExerciseId { get; set; }

    // Navigation properties
    public virtual Passage Passage { get; set; }
    public virtual Exercise Exercise { get; set; }
}
