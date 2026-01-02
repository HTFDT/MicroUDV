using OrderService.Domain.Storage.Abstractions;
using OrderService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomDbContext<OrderDbContext>(cfg =>
{
    cfg.ConnectionString = builder.Configuration["conn"]!;
})
    .AddRepository<IOrderRepository, OrderRepository>();

builder.Services.AddCqs();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await using var scope = app.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<OrderDbContext>(scope.ServiceProvider);

await app.RunAsync();