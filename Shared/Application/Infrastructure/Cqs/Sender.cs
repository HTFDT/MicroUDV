using MediatR;
using Shared.Application.Infrastructure.Results.Abstractions;
using ISender = Shared.Application.Infrastructure.Cqs.Abstractions.ISender;
using IRequest = Shared.Application.Infrastructure.Cqs.Abstractions.IRequest;

namespace Shared.Application.Infrastructure.Cqs;

public class Sender(MediatR.ISender sender) : ISender
{
    public Task<TResult> SendAsync<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
        where TResult : IResult
    {
        if (request is IRequest<TResult> mediatorRequest)
            return sender.Send(mediatorRequest, cancellationToken);
        throw new NotSupportedException("The request type is not supported.");
    }
}