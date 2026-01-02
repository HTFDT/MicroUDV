using Shared.Domain.Abstractions;

namespace InventoryService.Domain.Types;

public class Reservation : IEntity<Guid>
{
    public Guid Id { get; init; }
    public int Quantity { get; set; }
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
    public Guid ProductId { get; set;}
    public Product Product { get; set; } = null!;
}