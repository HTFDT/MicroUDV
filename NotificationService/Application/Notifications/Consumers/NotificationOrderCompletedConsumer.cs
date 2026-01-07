using NotificationService.Application.Notifications.Commands.CompleteOrder;
using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Orders.Messages.Events;
using Shared.MT.Application.Infrastructure.Messenging;


namespace NotificationService.Application.Notifications.Consumers;

public class NotificationOrderCompletedConsumer(ISender sender) : ConsumerBase<OrderCompleted>
{
    public override Task Consume()
    {
        return sender.SendAsync<CompleteOrderCommand, IResult>(new CompleteOrderCommand(ConsumeContext.Message));
    }
}