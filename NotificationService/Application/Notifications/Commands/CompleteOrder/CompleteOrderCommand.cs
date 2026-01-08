using NotificationService.Domain.Storage;
using Shared.Application.Infrastructure.Cqs;
using OrderCompletedMessage = Shared.Application.Orders.Messages.Events.OrderCompleted;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;
using NotificationService.Domain.Types;
using Shared.Application.Infrastructure.Results;

namespace NotificationService.Application.Notifications.Commands.CompleteOrder;

public class CompleteOrderCommand(OrderCompletedMessage message) : Command
{
    public OrderCompletedMessage Message { get; set; } = message;
}

public class CompleteOrderCommandHandler : CommandHandler<CompleteOrderCommand>
{
    private readonly INotificationRepository _repository;
    private readonly ILogger<CompleteOrderCommandHandler> _logger;

    public CompleteOrderCommandHandler(
        INotificationRepository repository,
        ILogger<CompleteOrderCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<IResult> HandleAsync(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating notification. UserId: {UserId}, Order successful: {IsSuccessful}",
            request.Message.UserId,
            request.Message.IsSuccessful);

        var notification = new Notification()
        {
            UserId = request.Message.UserId,
            Text = request.Message.IsSuccessful ? "Заказ успешно оформлен." : $"Заказ отменен. Причина:\n{request.Message.Reason}"
        };

        await _repository.AddAsync(notification, cancellationToken);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Notification created successfully for user: {UserId}",
            request.Message.UserId);

        return Result.Success();
    }
}