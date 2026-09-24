using Reminders.Application.TransferModels.Notification;
using Reminders.Domain.Models;

namespace Reminders.Application.Repositories.Interfaces;

public interface INotificationRepository
{
    public Task CreateNotificationAsync(Notification notification);

    public Task<List<Notification>> GetComingNotificationsAsync(CancellationToken stoppingToken);

    public Task UpdateNotificationsListAsync(List<Notification> notifications, CancellationToken stoppingToken);
    
    public Task<List<NotificationListDTO>> GetUserNotifications(Guid userId);
}