using MediatR;
using Shared.Application.Infrastructure.Results.Abstractions;
using ISender = Shared.Application.Infrastructure.Cqs.Abstractions.ISender;
using ICqsRequest = Shared.Application.Infrastructure.Cqs.Abstractions.ICqsRequest;

namespace Shared.Application.Infrastructure.Cqs;

public class Sender(MediatR.ISender sender) : ISender
{
    public Task<TResult> SendAsync<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : ICqsRequest
        where TResult : IResult
    {
        if (request is IRequest<TResult> mediatorRequest)
            return sender.Send(mediatorRequest, cancellationToken);
        throw new NotSupportedException("The request type is not supported.");
    }
}