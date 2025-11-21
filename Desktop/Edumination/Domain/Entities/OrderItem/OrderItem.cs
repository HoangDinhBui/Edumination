using Edumination.Api.Domain.Entities;
namespace Edumination.Api.Domain.Entities;

public class OrderItem
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long CourseId { get; set; }
    public int UnitVnd { get; set; }
    public int Qty { get; set; } = 1;
    public Order Order { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
