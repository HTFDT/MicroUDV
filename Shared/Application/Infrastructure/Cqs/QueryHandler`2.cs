using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Cqs;

public abstract class QueryHandler<TQuery, TResult> : HandlerBase<TQuery, IResult<TResult>>, IQueryHandler<TQuery, TResult>
    where TQuery : Query<TResult>
{
    Task<IResult<TResult>> IQueryHandler<TQuery, TResult>.HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        return HandleAsync(query, cancellationToken);
    }
}