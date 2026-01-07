using PaymentService.Domain.Storage.Abstractions;
using PaymentService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.Helpers;
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

builder.Services.AddCustomDbContext<PaymentDbContext>(cfg =>
{
    cfg.ConnectionString = builder.Configuration["conn"]!;
})
    .AddRepository<IPaymentRepository, PaymentRepository>();

builder.Services.AddCqs();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await using var scope = app.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<PaymentDbContext>(scope.ServiceProvider);

Log.Information("PaymentService starting...");

await app.RunAsync();