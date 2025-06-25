namespace InventoryService.Entities;

public interface InventoryRejected
{
    Guid OrderId { get; }
    string Reason { get; }
}
