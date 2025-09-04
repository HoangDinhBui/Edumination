namespace Edumination.Api.Features.Courses.Dtos;

public sealed class CreateCourseResponse
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string Level { get; set; } = default!;
    public bool IsPublished { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}
