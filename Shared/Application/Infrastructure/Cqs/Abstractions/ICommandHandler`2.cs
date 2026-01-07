using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Cqs.Abstractions;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<IResult<TResult>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}