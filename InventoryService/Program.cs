using InventoryService.Domain.Storage;
using InventoryService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomDbContext<InventoryDbContext>()
    .AddRepository<IProductRepository, ProductRepository>();

builder.Services.Configure<DbOptions>(o =>
{
    o.ConnectionString = builder.Configuration["conn"]!;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await using var scope = app.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<InventoryDbContext>(scope.ServiceProvider);

await app.RunAsync();