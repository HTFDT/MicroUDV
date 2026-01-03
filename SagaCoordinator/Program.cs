using SagaCoordinator.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.MT.Helpers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddCustomDbContext<SagasDbContext>(cfg =>
{
    cfg.ConnectionString = builder.Configuration["conn"]!;
});

builder.Services.AddMassTransitTransportWithSagas<SagasDbContext>(transportCfg =>
{
    if (builder.Environment.IsDevelopment())
    {
        transportCfg.IsInMemoryTransport = true;
        return;
    }
    transportCfg.RabbitMq = new TransportOptions.RabbitMqOptions
    {
        Host = builder.Configuration["RabbitMq:Host"]!,
        VirtualHost = builder.Configuration["RabbitMq:VirtualHost"]!,
        UserName = builder.Configuration["RabbitMq:UserName"]!,
        Password = builder.Configuration["RabbitMq:Password"]!,
    };
}, massTransitCfg =>
{
    massTransitCfg.StateMachinesConfig = new MassTransitOptions.SagaStateMachinesConfig
    {
        IsInMemoryPersistance = builder.Environment.IsDevelopment()
    };
});

var host = builder.Build();
host.Run();