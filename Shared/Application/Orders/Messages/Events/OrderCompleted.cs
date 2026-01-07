using Shared.Domain.Types;

namespace Shared.Application.Orders.Messages.Events;

public class OrderCompleted
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public bool IsSuccessful { get; set; }
    public string Reason { get; set; } = null!;
}