using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Cqs.Abstractions;

public interface ISender
{
    Task<TResult> SendAsync<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
        where TResult : IResult;
}