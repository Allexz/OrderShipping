using MassTransit;

namespace OrderService.Entities;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Created { get; set; }
    public List<OrderItem> Items { get; set; }
}
 