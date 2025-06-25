using MassTransit;
using OrderShipping.InventoryService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(rmq =>
{
    rmq.AddConsumer<InventoryConsumer>();
    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });
        cfg.ReceiveEndpoint("inventory-service", e =>
        {
            e.ConfigureConsumer<InventoryConsumer>(context); 
        });
    });

});

var app = builder.Build();

app.Run();
