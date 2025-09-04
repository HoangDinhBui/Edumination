namespace Edumination.Api.Features.Courses.Dtos;

public sealed class CourseListQuery
{
    public bool? published { get; set; }       // /courses?published=1
    public string? q { get; set; }             // tìm theo tiêu đề
    public string? level { get; set; }         // BEGINNER|... (optional)
    public int page { get; set; } = 1;
    public int pageSize { get; set; } = 20;
}

public sealed class CourseItemDto
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string Level { get; set; } = default!;
    public bool IsPublished { get; set; }
    public bool Enrolled { get; set; }    // true nếu user hiện tại đã enroll
}

public sealed class PagedResult<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
}
