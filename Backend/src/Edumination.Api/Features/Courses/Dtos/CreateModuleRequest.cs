namespace Edumination.Api.Features.Courses.Dtos;

public class CreateModuleRequest
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public int? Position { get; init; }  // null => append; >=1 => chèn và shift
}