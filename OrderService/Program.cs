using OrderService.Domain.Storage.Abstractions;
using OrderService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.Helpers;
using Shared.MT.Helpers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

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
EndpointConventionMapper.MapEndpoints();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await using var scope = app.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<OrderDbContext>(scope.ServiceProvider);

Log.Information("OrderService starting...");

await app.RunAsync();