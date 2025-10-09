namespace Edumination.Api.Domain.Entities;

public class Enrollment
{
    public long UserId { get; set; }
    public long CourseId { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
