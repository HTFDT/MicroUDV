using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Cqs;

public abstract class CommandHandler<TCommand> : HandlerBase<TCommand, IResult>, ICommandHandler<TCommand>
    where TCommand : Command
{
    Task<IResult> ICommandHandler<TCommand>.HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        return HandleAsync(command, cancellationToken);
    }
}