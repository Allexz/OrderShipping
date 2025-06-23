using MassTransit;
using ShippingService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit((rmq) =>
{
    rmq.AddConsumer<OrderPlacedConsumer>();
    // Register the bus as a singleton
    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });
        cfg.ReceiveEndpoint("shipping-order-queue", e =>
        {
            e.Consumer<OrderPlacedConsumer>(context);
            e.ConfigureConsumeTopology = false; 
            e.Bind("order-placed-exchange", x =>
            {
                x.ExchangeType = "direct";
                x.RoutingKey = "order.created";
            });
            e.UseKillSwitch(options =>             {
                options.SetActivationThreshold(2);  
                options.SetTripThreshold(2)
                .SetRestartTimeout(m: 1);  
            });

        });
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.Run();

