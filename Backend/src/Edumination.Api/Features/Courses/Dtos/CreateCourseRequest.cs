namespace Edumination.Api.Features.Courses.Dtos;

public sealed class CreateCourseRequest
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    // BEGINNER | ELEMENTARY | PRE_INTERMEDIATE | INTERMEDIATE | UPPER_INTERMEDIATE | ADVANCED
    public string Level { get; set; } = "BEGINNER";
    public bool IsPublished { get; set; } = false;
}
