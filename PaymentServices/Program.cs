using MassTransit;
using PaymentServices.Consumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });
        cfg.ReceiveEndpoint("payment-queue", e =>
        {
            e.Consumer<PaymentConsumer>();
        });
    });
});

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.


app.Run();

