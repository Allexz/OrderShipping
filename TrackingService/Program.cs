using MassTransit;
using SharedMessages.Messages;
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
        cfg.Message<OrderPlacedMessage>(x => x.SetEntityName("order-headers-exchange"));
        cfg.Publish<OrderPlacedMessage>(x => x.ExchangeType = "headers");

        cfg.ReceiveEndpoint("tracking-queue", e =>
        {
            e.Consumer<OrderPlacedConsumer>(context);
            e.Bind("order-headers-exchange", x =>
            {
                x.ExchangeType = "headers";
                x.SetBindingArgument("department", "tracking");
                x.SetBindingArgument("priority", "low");
                x.SetBindingArgument("x-match", "all");
            });
            e.PrefetchCount = 10; 
            e.ConcurrentMessageLimit = 5; 
        });

    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

