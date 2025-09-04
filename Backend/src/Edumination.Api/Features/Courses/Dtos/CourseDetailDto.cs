namespace Edumination.Api.Features.Courses.Dtos;

public sealed class LessonDto
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public int Position { get; set; }
    public bool IsPublished { get; set; }

    // Chỉ lộ khi CanViewContent = true
    public string? Objective { get; set; }
    public long? VideoId { get; set; }
    public long? TranscriptId { get; set; }
}

public sealed class ModuleDto
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public int Position { get; set; }

    // Danh sách bài học (ẩn bớt trường tùy quyền)
    public List<LessonDto> Lessons { get; set; } = new();
}

public sealed class CourseDetailDto
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string Level { get; set; } = default!;
    public bool IsPublished { get; set; }

    public bool Enrolled { get; set; }
    public bool CanViewContent { get; set; }

    // Outline chương trình (Module/Lesson). Nội dung chi tiết của Lesson
    // (Objective/VideoId/TranscriptId) chỉ populate khi CanViewContent = true.
    public List<ModuleDto> Modules { get; set; } = new();

    // Tiến độ (khi có user đăng nhập)
    public int? TotalLessons { get; set; }
    public int? CompletedLessons { get; set; }
}
