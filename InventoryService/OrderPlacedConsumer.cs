using MassTransit;
using SharedMessages.Messages;

namespace OrderShipping.InventoryService;
public class OrderPlacedConsumer: IConsumer<OrderPlacedMessage>
{
    public async Task Consume(ConsumeContext<OrderPlacedMessage> context)
    {
        // Simulate inventory update logic
        Console.WriteLine($"Inventory reserved for order: {context.Message.OrderId}, Quantity: {context.Message.Quantity}");
        
        await context.Publish(new InventoryReserved(context.Message.OrderId));
    }
}
