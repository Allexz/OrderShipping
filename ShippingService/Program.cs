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
            //e.UseMessageRetry(r=> r.Interval(3, TimeSpan.FromSeconds(5)));
            e.UseMessageRetry(r => r.Exponential(
                3,// retryLimit => m�ximo de tentativas antes de mover a mensagem para DEAD LETTER QUEUE
                TimeSpan.FromSeconds(1), // minInterval => intervalo inicial entre tentativas (1� retry ap�s 1 segundo)
                TimeSpan.FromSeconds(5), // maxInterval => intervalo m�ximo entre tentativas (n�o ultrapassa os 5 segundos configurados)
                TimeSpan.FromSeconds(10)));// intervalDelta => tempo total m�ximo de atraso acumulado antes de parar de esperar e avan�ar para o pr�ximo retry ou DEAD LETTER QUEUE
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

