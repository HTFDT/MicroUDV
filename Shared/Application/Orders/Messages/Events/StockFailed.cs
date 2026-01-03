namespace Shared.Application.Orders.Messages.Events;

public class StockFailed
{
    public Guid OrderId { get; set; }
    public string Reason { get; set; } = string.Empty;
}