namespace Shared.Application.Orders.Messages.Events;

public class OrderCreated
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderItemMessage> Items { get; set; } = null!;
}

public class OrderItemMessage
{
    public Guid OrderItemId { get; set; }
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}