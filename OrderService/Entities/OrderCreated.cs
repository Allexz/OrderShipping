namespace OrderService.Entities;

public interface OrderCreated
{
    Guid OrderId { get; set; }
    Guid CustomerId { get; set; }
    List<OrderItem> Items { get;set; }
    decimal TotalAmount { get;set; }
}
