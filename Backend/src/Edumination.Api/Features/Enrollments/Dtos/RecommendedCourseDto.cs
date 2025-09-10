namespace Edumination.Api.Features.Recommendations.Dtos;

public class RecommendedCourseDto
{
    public long CourseId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Level { get; set; } = null!;
    public bool IsPublished { get; set; }
    public decimal BandMin { get; set; }
    public decimal BandMax { get; set; }
    public decimal? Distance { get; set; } // độ “gần” so với target band (càng nhỏ càng tốt)
}
