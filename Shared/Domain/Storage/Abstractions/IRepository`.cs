using Shared.Domain.Abstractions;

namespace Shared.Domain.Storage.Abstractions;

public interface IRepository<T> : IReadOnlyRepository<T> where T : IEntity, IAggregateRoot
{
    Task AddAsync(T aggregateRoot, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> aggregateRoots, CancellationToken cancellationToken = default);
    Task RemoveAsync(T aggregateRoot, CancellationToken cancellationToken = default);
    Task RemoveRangeAsync(IEnumerable<T> aggregateRoots, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TResult>> QueryAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> predicate, CancellationToken cancellationToken = default);
    IUnitOfWork UnitOfWork { get; }
}