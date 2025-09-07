namespace Edumination.Api.Features.Courses.Dtos;

public class CreateLessonRequest
{
    public string Title { get; init; } = null!;
    public string? Objective { get; init; }
    public long? VideoId { get; init; }
    public long? TranscriptId { get; init; }
    public int? Position { get; init; }     // null => append; >=1 => chèn & shift
    public bool? IsPublished { get; init; } // null => mặc định false
}
