using NotificationService.Domain.Storage;
using NotificationService.Domain.Types;
using Shared.EF.Infrastructure;

namespace NotificationService.Infrastructure.Storage.EFCore;

public class NotificationRepository(NotificationDbContext dbContext) : EFRepository<Notification, NotificationDbContext>(dbContext), INotificationRepository;