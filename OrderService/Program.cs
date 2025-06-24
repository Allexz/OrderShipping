using MassTransit;
using SharedMessages.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
//EndpointConvention.Map<OrderPlacedMessage>(new Uri("queue:order-placed-exchange"));


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/orders", async (OrderDto order, IBus bus) =>
{
    var orderPlacedMessage = new OrderPlacedMessage(order.OrderId, order.Quantity);
    await bus.Publish(orderPlacedMessage);
    return Results.Created($"orders/{order.OrderId}", orderPlacedMessage);
});

app.Run();

public record OrderDto(Guid OrderId, int Quantity);