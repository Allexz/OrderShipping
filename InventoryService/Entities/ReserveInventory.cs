namespace InventoryService.Entities;

public interface ReserveInventory
{
    Guid OrderId { get; }
    List<Guid> Items { get; }
}
