namespace Edumination.Api.Domain.Entities;

public class Order
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public int TotalVnd { get; set; }
    public string Status { get; set; } = "PENDING"; // enum mapping
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}