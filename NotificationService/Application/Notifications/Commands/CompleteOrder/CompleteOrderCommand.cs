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

public class CompleteOrderCommandHandler(INotificationRepository repository) : CommandHandler<CompleteOrderCommand>
{
    protected override async Task<IResult> HandleAsync(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification()
        {
            UserId = request.Message.UserId,
            Text = request.Message.IsSuccessful ? "Заказ успешно оформлен" : "Заказ отменен"
        };

        await repository.AddAsync(notification, cancellationToken);
        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}