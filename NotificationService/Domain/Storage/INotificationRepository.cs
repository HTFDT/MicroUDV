using Shared.Domain.Storage.Abstractions;
using NotificationService.Domain.Types;

namespace NotificationService.Domain.Storage;

public interface INotificationRepository : IRepository<Notification>;