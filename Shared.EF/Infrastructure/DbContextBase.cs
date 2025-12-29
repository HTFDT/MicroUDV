using Microsoft.EntityFrameworkCore;
using Shared.Domain.Storage.Abstractions;

namespace Shared.EF.Infrastructure;

public abstract class DbContextBase(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return SaveChangesAsync(cancellationToken);
    }
}