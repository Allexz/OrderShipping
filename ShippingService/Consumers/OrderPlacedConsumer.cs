using MassTransit;
using SharedMessages.Messages;

namespace ShippingService.Consumers;

public class OrderPlacedConsumer : IConsumer<OrderPlacedMessage>
{   
    public Task Consume(ConsumeContext<OrderPlacedMessage> context)
    {
        if (context.Message.Quantity <= 0)
        {
            Console.WriteLine($"Rejected order with ID: {context.Message.OrderId}");
            throw new Exception("Order rejected due to insufficient stock.");
        }
        Console.WriteLine($"Order placed with ID: {context.Message.OrderId}, Quantity: {context.Message.Quantity}");
        return Task.CompletedTask;
    }
}
 