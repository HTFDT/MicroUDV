using Shared.Domain.Abstractions;

namespace NotificationService.Domain.Types;

public class Notification : IEntity<Guid>, IAggregateRoot
{
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public string Text { get; set; } = null!;
}