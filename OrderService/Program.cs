using MassTransit;
using SharedMessages.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(rmq =>
{
    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.ExchangeType = "fanout";
        //cfg.Publish<OrderPlacedMessage>(x => x.ExchangeType = "fanout");
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });

        cfg.Message<OrderPlacedMessage>(x => x.SetEntityName("order-placed-exchange"));

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

    await bus.Publish(orderPlacedMessage);

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