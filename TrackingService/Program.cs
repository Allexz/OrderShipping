using MassTransit;
using TrackingService.Consumers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(( rmq) =>
{
    rmq.AddConsumer<OrderPlacedConsumer>();
    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.ExchangeType = "direct";
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });

        cfg.ReceiveEndpoint("tracking-order-queue", e =>
        {
            e.ConfigureConsumer<OrderPlacedConsumer>(context);
            e.Bind("order-placed-exchange", x =>
            {
                x.RoutingKey = "order.tracking";
            });
            e.ConfigureConsumeTopology = false;
        });

    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

