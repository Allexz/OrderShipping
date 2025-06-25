using MassTransit;
using PaymentServices.Entities;

namespace PaymentServices.Consumer;

public class PaymentConsumer : IConsumer<ProcessPayment>
{
    public async Task Consume(ConsumeContext<ProcessPayment> context)
    {
        var paymentApproved = ProcessPayment(context.Message.Amount);

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