namespace Edumination.Api.Domain.Entities;

using Edumination.Api.Domain.Entities.Orders;

public class Order
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public int TotalVnd { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.PENDING;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}