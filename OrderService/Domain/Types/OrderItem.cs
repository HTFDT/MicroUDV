using Shared.Domain.Abstractions;

namespace OrderService.Domain.Types;

public class OrderItem : IEntity<Guid>
{
    public Guid Id { get; init; }
    public int Quantity { get; set; }
    
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public Guid ItemId { get; set; }
}