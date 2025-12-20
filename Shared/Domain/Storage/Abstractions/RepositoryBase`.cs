using Shared.Domain.Abstractions;

namespace Shared.Domain.Storage.Abstractions;

public abstract class RepositoryBase<T>(IUnitOfWork unitOfWork) : ReadOnlyRepositoryBase<T>, IRepository<T> where T : IEntity, IAggregateRoot
{
    public abstract Task AddAsync(T aggregateRoot, CancellationToken cancellationToken = default);
    public abstract Task AddRangeAsync(IEnumerable<T> aggregateRoots, CancellationToken cancellationToken = default);
    public abstract Task RemoveAsync(T aggregateRoot, CancellationToken cancellationToken = default);
    public abstract Task RemoveRangeAsync(IEnumerable<T> aggregateRoots, CancellationToken cancellationToken = default);
    public abstract Task<IReadOnlyList<TResult>> QueryAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> predicate, CancellationToken cancellationToken = default);


    public IUnitOfWork UnitOfWork
    {
        get
        {
            if (IsReadOnly)
            {
                throw new NotSupportedException("Repository is read-only.");
            }

            return unitOfWork;
        }
    }
}