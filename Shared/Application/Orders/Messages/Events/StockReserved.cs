using Shared.Application.Shared.Messages;

namespace Shared.Application.Orders.Messages.Events;

public class StockReserved
{
    public Guid OrderId { get; set; }
    public MoneyMessage Summary { get; set; } = null!;
}