namespace Edumination.Api.Features.Recommendations.Dtos;

public class CourseRecommendationQuery
{
    public int limit { get; set; } = 10;        // số item tối đa
    public string? level { get; set; }          // BEGINNER/ELEMENTARY/...
    public bool excludeEnrolled { get; set; } = true; // loại các course đã enroll
}
