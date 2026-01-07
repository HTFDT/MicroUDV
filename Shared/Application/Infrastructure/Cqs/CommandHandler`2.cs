using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Cqs;

public abstract class CommandHandler<TCommand, TResult> : HandlerBase<TCommand, IResult<TResult>>, ICommandHandler<TCommand, TResult>
    where TCommand : Command<TResult>
{
    Task<IResult<TResult>> ICommandHandler<TCommand, TResult>.HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        return HandleAsync(command, cancellationToken);
    }
}