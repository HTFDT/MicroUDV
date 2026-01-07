using InventoryService.Application.Products.Commands;
using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Orders.Messages.Events;
using Shared.MT.Application.Infrastructure.Messenging;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;

namespace InventoryService.Application.Products.Consumers;

public class InventoryOrderCompletedConsumer(ISender sender) : ConsumerBase<OrderCompleted>
{
    public override async Task Consume()
    {
        await sender.SendAsync<CompleteOrderCommand, IResult>(new CompleteOrderCommand(ConsumeContext.Message));
    }
}