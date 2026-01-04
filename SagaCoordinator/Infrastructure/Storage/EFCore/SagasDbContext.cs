using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.EF.Infrastructure;

namespace SagaCoordinator.Infrastructure.Storage.EFCore;

public class SagasDbContext(DbContextOptions options) : DbContextBase(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}