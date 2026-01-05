using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Types;
using Shared.EF.Infrastructure;
using Shared.MT.Infrastructure;

namespace PaymentService.Infrastructure.Storage.EFCore;

public class PaymentDbContext(DbContextOptions options) : MassTransitDbContextBase(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Payment>()
            .OwnsOne(p => p.Summary, b =>
            {
                b.Property("_value");
                b.Ignore(m => m.Rub);
            });
    }
}