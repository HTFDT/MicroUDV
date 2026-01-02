using PaymentService.Domain.Storage.Abstractions;
using PaymentService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.Helpers;

var builder = WebApplication.CreateBuilder(args);

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

await app.RunAsync();