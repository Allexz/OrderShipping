namespace PaymentServices.Entities;

public interface PaymentCompleted
{
    Guid OrderId { get; }
    Guid CustomerId { get; }
    decimal Amount { get; }
}
