using MassTransit;
using SharedMessages.Messages;

namespace TrackingService.Consumers;

public class OrderPlacedConsumer : IConsumer<OrderPlacedMessage>
{
    public async Task Consume(ConsumeContext<OrderPlacedMessage> context)
    {
		try
		{
			Console.WriteLine($"Order tracked: {context.Message.OrderId} - {context.Message.Quantity}");

			await Task.Delay(10000);
		}
		catch (Exception ex)
		{

			throw;
		}
    }
}
