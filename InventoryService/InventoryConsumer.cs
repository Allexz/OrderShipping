using InventoryService.Entities;
using MassTransit;

namespace OrderShipping.InventoryService;

public class InventoryConsumer : IConsumer<ReserveInventory>
{
    public async Task Consume(ConsumeContext<ReserveInventory> context)
    {
        var hasStock = CheckInventory(context.Message.Items);

        if (hasStock)
        {
            await context.Publish<InventoryReserved>(new
            {
                OrderId = context.Message.OrderId
            });
        }
        else
        {
            await context.Publish<InventoryRejected>(new
            {
                OrderId = context.Message.OrderId,
                Reason = "Insufficient inventory"
            });
        }
    }

    private bool CheckInventory(List<Guid> items)
    {
        return true;
    }
}

