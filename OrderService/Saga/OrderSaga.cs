using MassTransit;
using OrderService.Entities;
using InventoryService.Entities;
using PaymentServices.Entities;

namespace OrderService.Saga;

public class OrderSaga : MassTransitStateMachine<OrderState>
{
    public State Submitted { get; private set; }
    public State AwaitingInventory { get; private set; }
    public State AwaitingPayment { get; private set; }
    public State Completed { get; private set; }
    public State Canceled { get; private set; }

    public Event<OrderCreated> OrderCreatedEvent { get; private set; }
    public Event<InventoryReserved> InventoryReservedEvent { get; private set; }
    public Event<InventoryRejected> InventoryRejectedEvent { get; private set; }
    public Event<PaymentCompleted> PaymentCompletedEvent { get; private set; }

    public OrderSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreatedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => InventoryReservedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => InventoryRejectedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentCompletedEvent, x => x.CorrelateById(context => context.Message.OrderId));

        Initially(
            When(OrderCreatedEvent)
                .Then(context =>
                {
                    context.Instance.OrderId = context.Data.OrderId;
                    context.Instance.CustomerId = context.Data.CustomerId;
                    context.Instance.Amount = context.Data.TotalAmount;
                    context.Instance.Created = DateTime.UtcNow;
                    context.Instance.Items = context.Data.Items;
                })
                .PublishAsync(context => context.Init<ReserveInventory>(new
                {
                    OrderId = context.Instance.OrderId,
                    Items = context.Instance.Items
                }))
                .TransitionTo(AwaitingInventory));

        During(AwaitingInventory,
            When(InventoryReservedEvent)
                .PublishAsync(context => context.Init<ProcessPayment>(new
                {
                    OrderId = context.Instance.OrderId,
                    CustomerId = context.Instance.CustomerId,
                    Amount = context.Instance.Amount
                }))
                .TransitionTo(AwaitingPayment),
            When(InventoryRejectedEvent)
                .Then(context => Console.WriteLine($"Order {context.Instance.OrderId} rejected due to inventory"))
                .TransitionTo(Canceled));

        During(AwaitingPayment,
            When(PaymentCompletedEvent)
                .Then(context => Console.WriteLine($"Order {context.Instance.OrderId} completed"))
                .TransitionTo(Completed));
    }
}