using MassTransit;
using OrderService.Entities;
using OrderService.Saga;
using OrderService.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(rmq =>
{
    rmq.AddSagaStateMachine<OrderSaga, OrderState>()
        .InMemoryRepository();

    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });
        cfg.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter());
        cfg.ReceiveEndpoint("order-state", e =>
        {
            e.ConfigureSaga<OrderState>(context);
        });
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/orders", async (CreateOrderRequest request, IPublishEndpoint publishEndpoint) =>
{
    var orderId = NewId.NextGuid();

    await publishEndpoint.Publish<OrderCreated>(new
    {
        OrderId = orderId,
        CustomerId = request.CustomerId,
        Items = request.Items.Select(item => new
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            Price = item.Price
        }).ToList(),
        TotalAmount = request.Items.Sum(item => item.Quantity * item.Price)
    });

    return Results.Accepted($"/api/orders/{orderId}", new { OrderId = orderId });
})
.WithName("CreateOrder")
.WithOpenApi();

app.Run();

public record CreateOrderRequest(
    Guid CustomerId,
    List<OrderItemDto> Items);

public record OrderItemDto(
    Guid ProductId,
    int Quantity,
    decimal Price);