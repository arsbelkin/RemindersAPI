using Reminders.Application.TransferModels.Notification;
using Reminders.Domain.Models;
using Reminders.Domain.Enums;

namespace Reminders.Application.Repositories.Interfaces;

public interface INotificationRepository
{
    public Task CreateNotificationAsync(Notification notification);

    public Task<List<Notification>> GetComingNotificationsAsync();

    public Task UpdateNotificationsListAsync(List<Notification> notifications);

    public Task SetSentNotificationsByListIdAsync(List<Guid> notificationIds);

    public Task<List<NotificationListDTO>> GetUserNotifications(Guid userId);

    public Task<Notification?> GetNotification(Guid notificationId, Guid userId);

    public Task DeleteNotification(Notification notification);

    public Task<List<Notification>> GetNotificationsByReminder(Guid reminderId,
        ProcessedStatusTypes status);
    
    public Task DeleteNotificationByReminderId(Guid reminderId);
}