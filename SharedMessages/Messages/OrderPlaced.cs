namespace SharedMessages.Messages;
public interface OrderPlaced
{
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
}

public record OrderPlacedMessage(Guid OrderId, int Quantity) : OrderPlaced
{
    public Guid OrderId { get;set; } = OrderId;
    public int Quantity { get; set; } = Quantity;
}
