using Reminders.Application.TransferModels.Notification;

namespace Reminders.Application.Services.Interfaces;

public interface IRedisNotification
{
    public Task ProcessMessageAsync(NotificationWrapper message);
    public Task AddToQueueAsync(NotificationWrapper message);
}