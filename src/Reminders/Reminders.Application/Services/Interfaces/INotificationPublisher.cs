using Reminders.Application.TransferModels.Notification;

namespace Reminders.Application.Services.Interfaces;

public interface INotificationPublisher
{
    public Task SendAsync(NotificationMessageDTO message, CancellationToken cancellationToken);
}