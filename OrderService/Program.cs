using OrderService.Domain.Storage.Abstractions;
using OrderService.Infrastructure.Storage;
using OrderService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomDbContext<OrderDbContext>()
    .AddRepository<IOrderRepository, OrderRepository>();

builder.Services.Configure<DbOptions>(o =>
{
    o.ConnectionString = builder.Configuration["conn"]!;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await using var scope = app.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<OrderDbContext>(scope.ServiceProvider);

await app.RunAsync();