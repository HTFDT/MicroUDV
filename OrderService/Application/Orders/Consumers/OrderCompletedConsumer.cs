using OrderService.Domain.Storage.Abstractions;
using Shared.Application.Orders.Messages.Events;
using Shared.Domain.Types;
using Shared.MT.Application.Infrastructure.Messenging;

namespace OrderService.Application.Orders.Consumers;

public class OrderCompletedConsumer(IOrderRepository repository) : ConsumerBase<OrderCompleted>
{
    public override async Task Consume()
    {
        var msg = ConsumeContext.Message;
        var order = await repository.FirstOrDefaultAsync(o => o.Id == msg.OrderId);
        if (order is null)
            return;

        if (msg.IsSuccessful)
        {
            order.Status = OrderStatus.Completed;
        }
        else
        {
            await repository.RemoveAsync(order);
        }

        
        await repository.UnitOfWork.SaveChangesAsync();
    }
}