using System.Linq.Expressions;
using Shared.Domain.Abstractions;

namespace Shared.Domain.Storage.Abstractions;

public abstract class ReadOnlyRepositoryBase<T> : IReadOnlyRepository<T> where T : IEntity, IAggregateRoot
{
    public bool IsReadOnly { get; private set; }
    public abstract Task<T?> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);
    public abstract Task<T> FirstAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    public abstract Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    public abstract Task<T> SingleAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    public abstract Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    public abstract Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    public abstract Task<long> LongCountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    public abstract Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
}