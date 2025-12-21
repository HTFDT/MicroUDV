using MediatR;
using Shared.Application.Infrastructure.Results.Abstractions;
using Shared.Application.Infrastructure.Results.Helpers;

namespace Shared.Application.Infrastructure.Cqs;

public abstract class HandlerBase<T, TResult> : IRequestHandler<T, TResult> 
    where T : IRequest<TResult>
    where TResult : IResult
{

    async Task<TResult> IRequestHandler<T, TResult>.Handle(T message, CancellationToken cancellationToken)
    {
        var canHandle = await CanHandleAsync(message, cancellationToken);
        if (!canHandle.IsSuccessful)
            return canHandle;
        return await HandleAsync(message, cancellationToken);
    }
    
    protected abstract Task<TResult> HandleAsync(T request, CancellationToken cancellationToken);

    protected virtual Task<TResult> CanHandleAsync(T request, CancellationToken cancellationToken)
    {
        return Task.FromResult(ResultFactory.Success<TResult>());
    }
}