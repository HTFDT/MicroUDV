using Shared.Application.Shared.Messages;

namespace Shared.Application.Orders.Messages.Commands;

public class RequestPayment
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public MoneyMessage Summary { get; set; } = null!;
}