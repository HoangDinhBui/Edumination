namespace Edumination.Api.Domain.Entities.Orders;

public enum OrderStatus
{
    PENDING,
    PAID,
    CANCELLED,
    REFUNDED
}

public enum PaymentStatus
{
    INIT,
    SUCCESS,
    FAILED,
}

public enum PaymentProvider
{
    MOMO,
    VNPAY,
    STRIPE
}
