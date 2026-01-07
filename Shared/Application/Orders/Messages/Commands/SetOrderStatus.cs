using Shared.Domain.Types;

namespace Shared.Application.Orders.Messages.Commands;

public class SetOrderStatus
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
}