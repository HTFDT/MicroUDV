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

public class SetOrderStatusCommandHandler : CommandHandler<SetOrderStatusCommand>
{
    private readonly OrderRepository _repository;
    private readonly ILogger<SetOrderStatusCommandHandler> _logger;

    public SetOrderStatusCommandHandler(
        OrderRepository repository,
        ILogger<SetOrderStatusCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<IResult> HandleAsync(SetOrderStatusCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Setting order status. OrderId: {OrderId}, New status: {Status}",
            request.Message.OrderId,
            request.Message.Status);

        var order = await _repository.FirstOrDefaultAsync(o => o.Id == request.Message.OrderId, cancellationToken);

        if (order is null)
        {
            _logger.LogWarning(
                "Order not found. OrderId: {OrderId}",
                request.Message.OrderId);
            return Result.Error(new NotFoundError<Order>(request.Message.OrderId));
        }

        var oldStatus = order.Status;
        order.Status = request.Message.Status;

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Order status updated. OrderId: {OrderId}, From: {OldStatus}, To: {NewStatus}",
            request.Message.OrderId,
            oldStatus,
            request.Message.Status);

        return Result.Success();
    }
}