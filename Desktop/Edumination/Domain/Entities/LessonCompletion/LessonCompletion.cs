using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Domain.Entities;

public class LessonCompletion
{
    public long UserId { get; set; }
    public long LessonId { get; set; }
    public DateTime CompletedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
    public Lesson Lesson { get; set; } = default!;
}