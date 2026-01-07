using OrderService.Application.Orders.Commands.CompleteOrder;
using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Orders.Messages.Events;
using Shared.MT.Application.Infrastructure.Messenging;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;

namespace OrderService.Application.Orders.Consumers;

public class OrderCompletedConsumer(ISender sender) : ConsumerBase<OrderCompleted>
{
    public override async Task Consume()
    {
        await sender.SendAsync<CompleteOrderCommand, IResult>(new CompleteOrderCommand(ConsumeContext.Message));
    }
}