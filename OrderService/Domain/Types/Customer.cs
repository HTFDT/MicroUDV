using Shared.Domain.Abstractions;

namespace OrderService.Domain.Types;

public class Customer : IEntity<Guid>
{
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;

    public Order Order { get; set; } = null!;
}