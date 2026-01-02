using Shared.Domain.Abstractions;
using Shared.Domain.Types;

namespace InventoryService.Domain.Types;

public class Product : IEntity<Guid>, IAggregateRoot
{
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public Money Price { get; set; } = null!;
    public int InStockQuantity { get; set; }
}