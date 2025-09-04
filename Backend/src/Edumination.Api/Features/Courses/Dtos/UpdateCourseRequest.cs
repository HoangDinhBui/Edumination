namespace Edumination.Api.Features.Courses.Dtos;

public class UpdateCourseRequest
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? Level { get; init; }
    public bool? IsPublished { get; init; }
}
