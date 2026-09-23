using Reminders.Application.TransferModels.Notification;
using Reminders.Domain.Models;

namespace Reminders.Application.Repositories.Interfaces;

public interface INotificationRepository
{
    public Task CreateNotificationAsync(Notification notification);
    public Task<List<NotificationMessageDTO>> GetComingNotificationsAsync(CancellationToken stoppingToken);
}