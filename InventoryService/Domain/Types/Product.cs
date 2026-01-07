using Shared.Domain.Abstractions;
using Shared.Domain.Types;

namespace InventoryService.Domain.Types;

public class Product : IEntity<Guid>, IAggregateRoot
{
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public Money Price { get; set; } = null!;
    /// <summary>
    /// Кол-во товара в наличии, когда товар резервируется, это значение уменьшается
    /// </summary>
    public int InStockQuantity { get; set; }
    public List<Reservation> Reservations { get; set; } = [];
}