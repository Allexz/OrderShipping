using MassTransit;
using SharedMessages.Messages;
using ShippingService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit((rmq) =>
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

        cfg.ReceiveEndpoint("shipping-queue", e =>
        {
            e.Consumer<OrderPlacedConsumer>(context);
            e.Bind("order-headers-exchange", x =>
            {
                x.ExchangeType = "headers";
                x.SetBindingArgument("department", "shipping");
                x.SetBindingArgument("priority", "high");
                x.SetBindingArgument("x-match", "all");
            });
        });
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

