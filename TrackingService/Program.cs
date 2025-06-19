using MassTransit;
using TrackingService.Consumers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(( rmq) =>
{
    rmq.AddConsumer<OrderPlacedConsumer>();
    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });

        cfg.ReceiveEndpoint("tracking-order-queue", e =>
        {
            e.ConfigureConsumer<OrderPlacedConsumer>(context);
            e.ConfigureConsumeTopology = false;
            e.Bind("order-placed-exchange", x =>
            {
                x.ExchangeType = "topic";
                x.RoutingKey = "order.#";
                x.Durable = true;
                x.AutoDelete = false;
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

