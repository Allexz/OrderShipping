using MassTransit;
using SharedMessages.Messages;

namespace PaymentServices;

public class InventoryReservedConsumer : IConsumer<InventoryReserved>
{
    public async Task Consume(ConsumeContext<InventoryReserved> context)
    {
        Console.WriteLine($"Payment processed for Order {context.Message.OrderId}");
        await context.Publish(new PaymentCompleted(context.Message.OrderId));
    }
}

