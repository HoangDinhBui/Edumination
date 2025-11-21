namespace Edumination.Api.Domain.Entities;

public class Module
{
    public long Id { get; set; }
    public long CourseId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int Position { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Course Course { get; set; } = default!;
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
