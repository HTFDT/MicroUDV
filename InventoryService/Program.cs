using InventoryService.Domain.Storage;
using InventoryService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.Helpers;
using Serilog;
using Shared.MT.Helpers;

var builder = WebApplication.CreateBuilder(args);

var asm = typeof(Program).Assembly;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddUnitOfWorkDbcontext<InventoryDbContext>(cfg =>
{
    cfg.ConnectionString = builder.Configuration["conn"]!;
})
    .AddRepository<IProductRepository, ProductRepository>();

builder.Services.AddMassTransitCustom(b =>
{
    b.AddConsumers(asm);

    b.AddConfigureEndpointsCallback(o =>
    {
        o.UseInMemoryOutbox = true;
        o.UseRedelivery = true;
        o.Retries = 5;
    });

    if (!builder.Environment.IsDevelopment())
        b.AddEntityFrameworkOutbox<InventoryDbContext>();

    var transportCfg = b.Transport();

    transportCfg.UsingRabbitMq(o =>
    {
        o.Host = builder.Configuration["RabbitMq:Host"]!;
        o.VirtualHost = builder.Configuration["RabbitMq:VirtualHost"]!;
        o.UserName = builder.Configuration["RabbitMq:UserName"]!;
        o.Password = builder.Configuration["RabbitMq:Password"]!;
    });
});

builder.Services.AddControllers();
builder.Services.AddCqs(asm);
EndpointConventionMapper.MapEndpoints();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await using var scope = app.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<InventoryDbContext>(scope.ServiceProvider);

Log.Information("InventoryService starting...");

await app.RunAsync();