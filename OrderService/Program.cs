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
            h.Heartbeat(30);
            h.RequestedConnectionTimeout(10000);
            h.PublisherConfirmation = true;
        });
        cfg.Message<OrderPlacedMessage>(x => x.SetEntityName("order-placed-exchange"));
        cfg.Publish<OrderPlacedMessage>(x =>
        {
            x.ExchangeType = "topic";
            x.Durable = true;
            x.AutoDelete = false;
            x.Exclude = true;
        });
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapPost("/orders", async (OrderRequest order, IBus bus) =>
{
    OrderPlacedMessage orderPlacedMessage = new OrderPlacedMessage(order.OrderId, order.Quantity);

    bool isLowQuantity = order.Quantity < 10;

    if (isLowQuantity)
    {
        await bus.Publish(orderPlacedMessage, context =>
        {
            context.SetRoutingKey("order.shipping");
        });
    }
    else
    {
        await bus.Publish(orderPlacedMessage, context =>
        {
            context.SetRoutingKey("order.regular.tracking");
        });
    }
    return Results.Created($"orders/{order.OrderId}", orderPlacedMessage);
});

// Adicione este endpoint temporário no OrderService para testar a conexão
app.MapGet("/test-rabbit", async (IBus bus) =>
{
    try
    {
        Guid guid = Guid.NewGuid();
        int quantity = 5;
        await bus.Publish(new OrderPlacedMessage(guid, quantity));
        return Results.Ok("Mensagem publicada com sucesso");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Falha ao publicar: {ex}");
    }
});

app.Run();

public record OrderRequest(Guid OrderId, int Quantity);