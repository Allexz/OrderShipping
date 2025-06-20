namespace SharedMessages.Messages;
public interface IOrderPlacedMessage
{
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
}

public record OrderPlacedMessage(Guid OrderId, int Quantity) : IOrderPlacedMessage
{
    public Guid OrderId { get; set; } = OrderId;
    public int Quantity { get; set; } = Quantity;
}
