using OrderService.Application.Orders.Commands.SetOrderStatus;
using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Orders.Messages.Commands;
using Shared.MT.Application.Infrastructure.Messenging;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;

namespace OrderService.Application.Orders.Consumers;

public class SetOrderStatusConsumer(ISender sender) : ConsumerBase<SetOrderStatus>
{
    public override Task Consume()
    {
        return sender.SendAsync<SetOrderStatusCommand, IResult>(new SetOrderStatusCommand(ConsumeContext.Message));
    }
}