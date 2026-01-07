using Shared.Application.Infrastructure.Cqs;
using SetOrderStatusMessage = Shared.Application.Orders.Messages.Commands.SetOrderStatus;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;
using OrderService.Infrastructure.Storage.EFCore;
using Shared.Application.Infrastructure.Results;
using Shared.Application.Shared.Results.Errors;
using OrderService.Domain.Types;

namespace OrderService.Application.Orders.Commands.SetOrderStatus;

public class SetOrderStatusCommand(SetOrderStatusMessage message) : Command
{
    public SetOrderStatusMessage Message { get; set; } = message;
}

public class SetOrderStatusCommandHandler(OrderRepository repository) : CommandHandler<SetOrderStatusCommand>
{
    protected override async Task<IResult> HandleAsync(SetOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.FirstOrDefaultAsync(o => o.Id == request.Message.OrderId, cancellationToken);

        if (order is null)
            return Result.Error(new NotFoundError<Order>(request.Message.OrderId));

        order.Status = request.Message.Status;

        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}