namespace SharedMessages.Messages;

public interface IInventoryReservedMessage
{
    public Guid OrderId { get; }
}   
public record InventoryReserved(Guid OrderId): IInventoryReservedMessage;
