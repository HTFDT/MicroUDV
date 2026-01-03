using InventoryService.Domain.Types;
using Microsoft.EntityFrameworkCore;
using Shared.EF.Infrastructure;

namespace InventoryService.Infrastructure.Storage.EFCore;

public class InventoryDbContext(DbContextOptions options) : DbContextBase(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .OwnsOne(p => p.Price, b =>
            {
                b.Property("_value");
                b.Ignore(m => m.Rub);
            });

        modelBuilder.Entity<Product>()
            .Navigation(p => p.Reservations)
            .AutoInclude();

        modelBuilder.Entity<Reservation>()
            .Navigation(r => r.Product)
            .AutoInclude();
    }
}