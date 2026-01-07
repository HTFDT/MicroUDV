using InventoryService.Application.Products.Commands;
using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Infrastructure.Results.Abstractions;
using Shared.Application.Orders.Messages.Commands;
using Shared.Application.Orders.Messages.Events;
using Shared.Application.Shared.Messages;
using Shared.Domain.Types;
using Shared.MT.Application.Infrastructure.Messenging;

namespace InventoryService.Application.Products.Consumers;

public class ReserveStockConsumer(ISender sender) : ConsumerBase<ReserveStock>
{
    public override async Task Consume()
    {
        var res = await sender.SendAsync<ReserveStockCommand, IResult<Money>>(new ReserveStockCommand(ConsumeContext.Message));
        if (!res.IsSuccessful)
        {
            await ConsumeContext.RespondAsync(new StockFailed
            {
                OrderId = ConsumeContext.Message.OrderId
            });
            return;
        }
            
        await ConsumeContext.RespondAsync(new StockReserved
        {
            OrderId = ConsumeContext.Message.OrderId,
            Summary = new MoneyMessage(res.Value)
        });
    }
}