using MassTransit;
using SharedMessages.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(rmq =>
{
    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });
    });
});
EndpointConvention.Map<OrderPlaced>(new Uri("queue:order-placed"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/orders", async (OrderRequest order, IBus bus) =>
{
    var orderPlacedMessage = new OrderPlaced(order.OrderId, order.Quantity);
    await bus.Send(orderPlacedMessage);

    return Results.Created($"orders/{order.OrderId}", orderPlacedMessage);
});

app.Run();

public record OrderRequest(Guid OrderId, int Quantity);