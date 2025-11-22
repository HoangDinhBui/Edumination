using System;
using System.Collections.Generic;
using Education.Domain.Entities;
using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Domain.Entities;

public class Lesson
{
    public long Id { get; set; }
    public long ModuleId { get; set; }
    public string Title { get; set; } = default!;
    public string? Objective { get; set; }
    public long? VideoId { get; set; }
    public long? TranscriptId { get; set; }
    public int Position { get; set; }
    public bool IsPublished { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    // public Module Module { get; set; } = default!;
    // public User CreatedByUser { get; set; } = default!;
    // public Asset? Video { get; set; }
    // public Asset? Transcript { get; set; }
    // public ICollection<LessonCompletion> Completions { get; set; } = new List<LessonCompletion>();
}
