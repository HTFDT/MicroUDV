using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Types;
using Shared.EF.Infrastructure;
using Shared.MT.Infrastructure;

namespace NotificationService.Infrastructure.Storage.EFCore;

public class NotificationDbContext(DbContextOptions options) : MassTransitDbContextBase(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Notification>();
    }
}