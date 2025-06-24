using MassTransit;
using SagaOrchestration;
using SharedMessages.Messages;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    //State
    public State Submitted { get; private set; }
    public State InventoryReserved { get; private set; }
    public State PaymentCompleted { get; private set; }

    //Events
    public Event<OrderPlacedMessage> OrderPlacedEvent { get; private set; }
    public Event<InventoryReserved> InventoryReservedEvent { get; private set; }

    public Event<PaymentCompleted> PaymentCompletedEvent { get; private set; }

    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);
        Event(()=> OrderPlacedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => InventoryReservedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentCompletedEvent, x => x.CorrelateById(context => context.Message.OrderId));

        Initially(
            When(OrderPlacedEvent)
                .Then(context =>
                {
                    context.Instance.OrderId = context.Data.OrderId;
                    context.Instance.Quantity = context.Data.Quantity;
                    Console.WriteLine($"Order placed: {context.Data.OrderId}, Quantity: {context.Data.Quantity}");
                })
                .TransitionTo(Submitted));
        During(Submitted,
            When(InventoryReservedEvent)
                .Then(context =>
                {
                    Console.WriteLine($"Inventory reserved for Order: {context.Instance.OrderId}");
                })
                .TransitionTo(InventoryReserved),
            When(PaymentCompletedEvent)
                .Then(context =>
                {
                    Console.WriteLine($"Payment completed for Order: {context.Instance.OrderId}");
                })
                .TransitionTo(PaymentCompleted)
                .Finalize());

        SetCompletedWhenFinalized();



    }
}