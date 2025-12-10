namespace PaymentServices.Entities;

public interface ProcessPayment
{
    Guid OrderId { get; }
    Guid CustomerId { get; }
    decimal Amount { get; }
}
