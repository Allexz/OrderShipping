namespace SharedMessages.Messages;

public interface IPaymentComplete
{
    public Guid OrderId { get; }
}
public record PaymentCompleted(Guid OrderId): IPaymentComplete;
 