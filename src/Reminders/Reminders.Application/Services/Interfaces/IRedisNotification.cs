using Reminders.Application.TransferModels.Notification;

namespace Reminders.Application.Services.Interfaces;

public interface IRedisNotification
{
    public Task AddToQueueAsync(NotificationWrapper message);
}