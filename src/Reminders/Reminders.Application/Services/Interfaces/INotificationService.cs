using Reminders.Application.TransferModels.Notification;

namespace Reminders.Application.Services.Interfaces;

public interface INotificationService
{
    public Task<Guid> CreateNotificationAsync(NotificationCreateDTO dto, Guid userId);
    public Task<List<NotificationListDTO>> GetUserNotifications(Guid userId);
    public Task DeleteNotification(Guid notificationId, Guid userId);
}