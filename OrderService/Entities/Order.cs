namespace OrderService.Entities;

public class Order
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}
