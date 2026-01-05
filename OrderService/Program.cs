using OrderService.Domain.Storage.Abstractions;
using OrderService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.Helpers;
using Shared.MT.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomDbContext<OrderDbContext>(cfg =>
{
    cfg.ConnectionString = builder.Configuration["conn"]!;
})
    .AddRepository<IOrderRepository, OrderRepository>();

builder.Services.AddMassTransitCustom(b =>
{
    b.AddConsumers();

    b.AddConfigureEndpointsCallback(o =>
    {
        o.UseInMemoryOutbox = true;
        o.UseRedelivery = true;
        o.Retries = 5;
    });

    if (!builder.Environment.IsDevelopment())
        b.AddEntityFrameworkOutbox<OrderDbContext>();

    var transportCfg = b.Transport();

    if (builder.Environment.IsDevelopment())
        transportCfg.UsingInMemory();
    else
        transportCfg.UsingRabbitMq(o =>
        {
            o.Host = builder.Configuration["RabbitMq:Host"]!;
            o.VirtualHost = builder.Configuration["RabbitMq:VirtualHost"]!;
            o.UserName = builder.Configuration["RabbitMq:UserName"]!;
            o.Password = builder.Configuration["RabbitMq:Password"]!;
        });
});

builder.Services.AddCqs();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await using var scope = app.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<OrderDbContext>(scope.ServiceProvider);

await app.RunAsync();