using Microsoft.EntityFrameworkCore;
using Shared.Domain.Storage.Abstractions;

namespace Shared.EF.Infrastructure;

public abstract class DbContextBase : DbContext, IUnitOfWork
{
    Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(cancellationToken);
    }
}