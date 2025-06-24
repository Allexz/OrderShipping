using MassTransit;
using OrderShipping.InventoryService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(rmq =>
{
    rmq.AddConsumer<OrderPlacedConsumer>();
    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });
    });
});

var app = builder.Build();

app.Run();
