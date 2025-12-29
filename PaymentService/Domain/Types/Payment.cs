using Shared.Domain.Abstractions;
using Shared.Domain.Storage.Types;

namespace PaymentService.Domain.Types;

public class Payment : IEntity<Guid>, IAggregateRoot
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public Money Summary { get; set; } = null!;
}