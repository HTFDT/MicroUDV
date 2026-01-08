using MassTransit;
using SagaCoordinator.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.MT.Helpers;

var builder = Host.CreateApplicationBuilder(args);

var asm = typeof(Program).Assembly;

builder.Services.AddCustomDbContext<SagasDbContext>(cfg =>
{
    cfg.ConnectionString = builder.Configuration["conn"]!;
});

builder.Services.AddMassTransitCustom(b =>
{
    b.AddConsumers(asm);
    var smCfg = b.AddSagaStateMachines(asm);
    
    if (builder.Environment.IsDevelopment())
        smCfg.UsingInMemoryRepository();
    else
        smCfg.UsingEntityFrameworkRepository<SagasDbContext>();

    b.AddConfigureEndpointsCallback(o =>
    {
        o.UseInMemoryOutbox = true;
        o.UseRedelivery = true;
        o.Retries = 5;
    });

    if (!builder.Environment.IsDevelopment())
        b.AddEntityFrameworkOutbox<SagasDbContext>();
    
    var transportCfg = b.Transport();

    transportCfg.UsingRabbitMq(o =>
    {
        o.Host = builder.Configuration["RabbitMq:Host"]!;
        o.VirtualHost = builder.Configuration["RabbitMq:VirtualHost"]!;
        o.UserName = builder.Configuration["RabbitMq:UserName"]!;
        o.Password = builder.Configuration["RabbitMq:Password"]!;
    });
});

EndpointConventionMapper.MapEndpoints();

var host = builder.Build();

await using var scope = host.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<SagasDbContext>(scope.ServiceProvider);

host.Run();