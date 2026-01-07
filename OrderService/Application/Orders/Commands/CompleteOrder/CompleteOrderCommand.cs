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

public class CompleteOrderCommandHandler : CommandHandler<CompleteOrderCommand>
{
    private readonly IOrderRepository _repository;
    private readonly ILogger<CompleteOrderCommandHandler> _logger;

    public CompleteOrderCommandHandler(
        IOrderRepository repository,
        ILogger<CompleteOrderCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<IResult> HandleAsync(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Completing order. OrderId: {OrderId}, IsSuccessful: {IsSuccessful}",
            request.Message.OrderId,
            request.Message.IsSuccessful);

        var order = await _repository.FirstOrDefaultAsync(o => o.Id == request.Message.OrderId, cancellationToken);
        if (order is null)
        {
            _logger.LogWarning(
                "Order not found. OrderId: {OrderId}",
                request.Message.OrderId);

            return Result.Error(new NotFoundError<Order>(request.Message.OrderId));
        }

        var oldStatus = order.Status;
        var newStatus = request.Message.IsSuccessful
            ? OrderStatus.Completed
            : OrderStatus.Canceled;

        order.Status = newStatus;

        _logger.LogInformation(
            "Changing order status. OrderId: {OrderId}, From: {OldStatus}, To: {NewStatus}",
            request.Message.OrderId,
            oldStatus,
            newStatus);

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Order completed successfully. OrderId: {OrderId}",
            request.Message.OrderId);

        return Result.Success();
    }
}