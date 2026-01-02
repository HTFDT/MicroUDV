using PaymentService.Domain.Storage.Abstractions;
using PaymentService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomDbContext<PaymentDbContext>()
    .AddRepository<IPaymentRepository, PaymentRepository>();

builder.Services.AddCqs();

builder.Services.Configure<DbOptions>(o =>
{
    o.ConnectionString = builder.Configuration["conn"]!;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await using var scope = app.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<PaymentDbContext>(scope.ServiceProvider);

await app.RunAsync();