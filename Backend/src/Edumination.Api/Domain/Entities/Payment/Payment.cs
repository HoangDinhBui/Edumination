using Edumination.Api.Domain.Entities;
namespace Edumination.Api.Domain.Entities;

public class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string Provider { get; set; } = "MOMO";
    public string? ProviderTxnId { get; set; }
    public int AmountVnd { get; set; }
    public string Status { get; set; } = "INIT";
    public string? RawResponse { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Order Order { get; set; } = null!;
}