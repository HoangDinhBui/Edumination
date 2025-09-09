namespace Edumination.Api.Features.Enrollments.Dtos;

public class MyEnrollmentQuery
{
    public int page { get; set; } = 1;
    public int pageSize { get; set; } = 20;

    // tuỳ chọn lọc
    public string? q { get; set; }          // search theo title
    public bool? published { get; set; }    // lọc theo is_published
    public string? level { get; set; }      // BEGINNER/INTERMEDIATE/...
}
