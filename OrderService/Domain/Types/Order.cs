using Shared.Domain.Abstractions;

namespace OrderService.Domain.Types;

public class Order : IEntity<Guid>, IAggregateRoot
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; set; }
    public OrderStatus Status { get; set; }
    public Address Address { get; set; } = null!;
    
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public IList<OrderItem> Items { get; set; } = [];
}