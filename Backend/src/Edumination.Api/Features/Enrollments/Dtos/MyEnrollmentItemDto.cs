namespace Edumination.Api.Features.Enrollments.Dtos;

public class MyEnrollmentItemDto
{
    public long CourseId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Level { get; set; } = null!;
    public bool IsPublished { get; set; }
    public DateTime EnrolledAt { get; set; }

    // tiến độ (có thể null nếu khoá chưa có lesson)
    public int? TotalLessons { get; set; }
    public int? CompletedLessons { get; set; }
}
