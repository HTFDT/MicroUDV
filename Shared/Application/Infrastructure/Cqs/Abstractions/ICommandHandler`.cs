using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Cqs.Abstractions;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<IResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}