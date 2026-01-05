using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.EF.Infrastructure;

namespace Shared.MT.Infrastructure;

public abstract class MassTransitDbContextBase(DbContextOptions options) : DbContextBase(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}