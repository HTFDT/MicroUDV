using InventoryService.Domain.Storage;
using InventoryService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomDbContext<InventoryDbContext>(cfg =>
{
    cfg.ConnectionString = builder.Configuration["conn"]!;
})
    .AddRepository<IProductRepository, ProductRepository>();

builder.Services.AddCqs();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await using var scope = app.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<InventoryDbContext>(scope.ServiceProvider);

await app.RunAsync();