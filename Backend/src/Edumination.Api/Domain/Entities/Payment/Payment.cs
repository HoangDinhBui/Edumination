using Edumination.Api.Domain.Entities;
using Edumination.Api.Domain.Entities.Orders;
namespace Edumination.Api.Domain.Entities;

public class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public PaymentProvider Provider { get; set; }
    public string? ProviderTxnId { get; set; }
    public int AmountVnd { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.INIT;
    public string? RawResponse { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Order Order { get; set; } = null!;
}