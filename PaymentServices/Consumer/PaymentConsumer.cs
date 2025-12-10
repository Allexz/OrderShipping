using MassTransit;
using PaymentServices.Entities;

namespace PaymentServices.Consumer;

public class PaymentConsumer : IConsumer<ProcessPayment>
{
    public async Task Consume(ConsumeContext<ProcessPayment> context)
    {
        var paymentApproved = ProcessPayment(context.Message.Amount);
        Console.WriteLine($"Processing payment for Order ID: {context.Message.OrderId}, Amount: {context.Message.Amount}");
        if (paymentApproved)
        {
            await context.Publish<PaymentCompleted>(new
            {
                context.Message.OrderId,
                context.Message.CustomerId,
                context.Message.Amount
            });
        }
    }

    private bool ProcessPayment(decimal amount)
    {
        return true;
    }
}