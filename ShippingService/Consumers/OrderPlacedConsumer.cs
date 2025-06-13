using MassTransit;
using SharedMessages.Messages;

namespace ShippingService.Consumers;

public class OrderPlacedConsumer : IConsumer<OrderPlaced>
{   
    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        // Here you would implement the logic to handle the OrderPlaced event
        // For example, you might log the order details or initiate shipping processing
        Console.WriteLine($"Order received: {context.Message.OrderId}, Quantity: {context.Message.Quantity}");
        return Task.CompletedTask;
    }
}
 