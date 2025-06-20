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
        cfg.Message<OrderPlacedMessage>(x => x.SetEntityName("order-placed-exchange"));
        cfg.Publish<OrderPlacedMessage>(exch => exch.ExchangeType = "direct");
        

    });
});
EndpointConvention.Map<OrderPlacedMessage>(new Uri("queue:order-placed-exchange"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/orders", async (OrderRequest order, IBus bus) =>
{
    var orderPlacedMessage = new OrderPlacedMessage(order.OrderId, order.Quantity);
    await bus.Publish(orderPlacedMessage, context =>
    {
        context.SetRoutingKey("order.created");
    });

    return Results.Created($"orders/{order.OrderId}", orderPlacedMessage);
});

app.Run();

public record OrderRequest(Guid OrderId, int Quantity);