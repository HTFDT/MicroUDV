using NotificationService.Domain.Storage;
using NotificationService.Infrastructure.Storage.EFCore;
using Shared.EF.Helpers;
using Shared.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomDbContext<NotificationDbContext>(cfg =>
{
    cfg.ConnectionString = builder.Configuration["conn"]!;
})
    .AddRepository<INotificationRepository, NotificationRepository>();

builder.Services.AddCqs();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await using var scope = app.Services.CreateAsyncScope();
DatabaseUpdater.UpdateDatabase<NotificationDbContext>(scope.ServiceProvider);

await app.RunAsync();