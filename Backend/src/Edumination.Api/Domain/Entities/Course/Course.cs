namespace Edumination.Api.Domain.Entities;

public enum CourseLevel
{
    BEGINNER,
    ELEMENTARY,
    PRE_INTERMEDIATE,
    INTERMEDIATE,
    UPPER_INTERMEDIATE,
    ADVANCED
}

public class Course
{
    public long Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public CourseLevel Level { get; set; } = CourseLevel.BEGINNER;
    public bool IsPublished { get; set; } = false;
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Enrollments> Enrollments { get; set; } = new List<Enrollments>();
}
