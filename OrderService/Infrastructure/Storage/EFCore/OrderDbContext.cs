using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Types;
using Shared.EF.Infrastructure;

namespace OrderService.Infrastructure.Storage.EFCore;

public class OrderDbContext(DbContextOptions options) : DbContextBase(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Order>()
            .HasOne<Customer>(o => o.Customer)
            .WithOne(c => c.Order)
            .HasForeignKey<Order>(o => o.CustomerId)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .HasMany<OrderItem>(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .OwnsOne(o => o.Address);

        modelBuilder.Entity<Order>()
            .Navigation(o => o.Customer)
            .AutoInclude();
        
        modelBuilder.Entity<Order>()
            .Navigation(o => o.Items)
            .AutoInclude();
        
        modelBuilder.Entity<Customer>()
            .Navigation(c => c.Order)
            .AutoInclude();
        
        modelBuilder.Entity<OrderItem>()
            .Navigation(o => o.Order)
            .AutoInclude();
    }
}