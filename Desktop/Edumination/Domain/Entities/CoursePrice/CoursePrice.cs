using Edumination.Api.Domain.Entities;
namespace Edumination.Api.Domain.Entities;

public class CoursePrice
{
    public long CourseId { get; set; }
    public int PriceVnd { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Course Course { get; set; } = null!;
}