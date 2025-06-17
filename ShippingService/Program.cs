using MassTransit;
using ShippingService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit((rmq) =>
{
    rmq.AddConsumer<OrderPlacedConsumer>();
    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.ExchangeType = "fanout"; 
        //cfg.Publish<OrderPlacedConsumer>(x => x.ExchangeType = "fanout");
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });

        cfg.ReceiveEndpoint("shipping-order-queue", e =>
        {
            e.ConfigureConsumer<OrderPlacedConsumer>(context);
            e.Bind("order-placed-exchange", x =>
            {
                x.ExchangeType = "fanout";
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

