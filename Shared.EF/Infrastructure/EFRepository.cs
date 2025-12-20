using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions;
using Shared.Domain.Storage.Abstractions;

namespace Shared.EF.Infrastructure;

public abstract class EFRepository<TAggregateRoot, TDbContext> : RepositoryBase<TAggregateRoot>
    where TAggregateRoot : class, IEntity, IAggregateRoot
    where TDbContext : DbContext, IUnitOfWork
{
    private readonly TDbContext _context;
    protected DbSet<TAggregateRoot> Items => _context.Set<TAggregateRoot>();
    protected IQueryable<TAggregateRoot> Query => IsReadOnly ? Items.AsNoTracking().AsQueryable() : Items.AsQueryable();
    
    public EFRepository(TDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

    public override Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default)
    {
        Items.Add(aggregateRoot);
        return Task.CompletedTask;
    }

    public override Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken = default)
    {
        Items.AddRange(aggregateRoots);
        return Task.CompletedTask;
    }

    public override Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default)
    {
        Items.Remove(aggregateRoot);
        return Task.CompletedTask;
    }

    public override Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken = default)
    {
        Items.RemoveRange(aggregateRoots);
        return Task.CompletedTask;
    }

    public override Task<TAggregateRoot?> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
    {
        var vt = Items.FindAsync(keyValues, cancellationToken);
        return vt.AsTask();
    }

    public override Task<TAggregateRoot> FirstAsync(Expression<Func<TAggregateRoot, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return predicate is null
            ? Query.FirstAsync(cancellationToken) 
            : Query.FirstAsync(predicate, cancellationToken);
    }

    public override Task<TAggregateRoot?> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return predicate is null
            ? Query.FirstOrDefaultAsync(cancellationToken) 
            : Query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public override Task<TAggregateRoot> SingleAsync(Expression<Func<TAggregateRoot, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return predicate is null
            ? Query.SingleAsync(cancellationToken) 
            : Query.SingleAsync(predicate, cancellationToken);
    }

    public override Task<TAggregateRoot?> SingleOrDefaultAsync(Expression<Func<TAggregateRoot, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return predicate is null
            ? Query.SingleOrDefaultAsync(cancellationToken) 
            : Query.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public override async Task<IReadOnlyList<TAggregateRoot>> ListAsync(Expression<Func<TAggregateRoot, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return await (predicate is null
            ? Query.ToListAsync(cancellationToken)
            : Query.Where(predicate).ToListAsync(cancellationToken));
    }

    public override Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return predicate is null
            ? Query.CountAsync(cancellationToken) 
            : Query.CountAsync(predicate, cancellationToken);
    }
    
    public override Task<long> LongCountAsync(Expression<Func<TAggregateRoot, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return predicate is null
            ? Query.LongCountAsync(cancellationToken) 
            : Query.LongCountAsync(predicate, cancellationToken);
    }

    public override async Task<IReadOnlyList<TResult>> QueryAsync<TResult>(Func<IQueryable<TAggregateRoot>, IQueryable<TResult>> predicate, CancellationToken cancellationToken = default)
    {
        return await predicate(Query).ToListAsync(cancellationToken);
    }
}