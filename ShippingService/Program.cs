using MassTransit;
using SharedMessages.Messages;
using ShippingService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit((rmq) =>
{
    rmq.AddConsumer<OrderPlacedConsumer>();
    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.ExchangeType = "topic"; // Define o tipo de exchange como topic
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });
        // Define explicitamente o tipo da exchange de publicação
        cfg.Publish<OrderPlaced>(x =>
        {
            x.ExchangeType = "topic";
        });

        cfg.ReceiveEndpoint("shipping-queue", e =>
        {
            e.ConfigureConsumeTopology = false;  
            e.ConfigureConsumer<OrderPlacedConsumer>(context);
            e.Bind("order-placed-exchange", x =>
            {
                x.ExchangeType = "topic"; 
                x.RoutingKey = "order.*";
            });
            e.PrefetchCount = 10; // Controla o número de mensagens pré-buscadas
            e.ConcurrentMessageLimit = 5; // Limite de mensagens processadas simultaneamente
        });
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

