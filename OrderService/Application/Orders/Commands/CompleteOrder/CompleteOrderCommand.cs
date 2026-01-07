using OrderService.Domain.Storage.Abstractions;
using OrderService.Domain.Types;
using Shared.Application.Infrastructure.Cqs;
using Shared.Application.Infrastructure.Results;
using Shared.Application.Orders.Messages.Events;
using Shared.Application.Shared.Results.Errors;
using Shared.Domain.Types;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;

namespace OrderService.Application.Orders.Commands.CompleteOrder;

public class CompleteOrderCommand(OrderCompleted message) : Command
{
    public OrderCompleted Message { get; set; } = message;
}

public class CompleteOrderCommandHandler(IOrderRepository repository) : CommandHandler<CompleteOrderCommand>
{
    protected override async Task<IResult> HandleAsync(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.FirstOrDefaultAsync(o => o.Id == request.Message.OrderId, cancellationToken);
        if (order is null)
            return Result.Error(new NotFoundError<Order>(request.Message.OrderId));

        order.Status = request.Message.IsSuccessful ? OrderStatus.Completed : OrderStatus.Canceled;

        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}