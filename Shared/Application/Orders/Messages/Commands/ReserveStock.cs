namespace Shared.Application.Orders.Messages.Commands;

public class ReserveStock
{
    public Guid OrderId { get; set; }
    public List<StockItemMessage> Products { get; set; } = null!;
}

public class StockItemMessage
{
    public Guid OrderItemId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}