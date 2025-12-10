using MassTransit;
using SagaOrchestration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(rmq =>
{
    rmq
    .AddSagaStateMachine<OrderStateMachine, OrderState>()
    .InMemoryRepository();

    rmq.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("admin");
            h.Password("senhaadmin");
        });
    });
});

// Add services to the container.

var app = builder.Build();

 
app.Run();

 
