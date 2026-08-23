using Reminders.Application.TransferModels.Notification;

namespace Reminders.Application.Services.Interfaces;

public interface INotificationService
{
    public Task<Guid> CreateNotificationAsync(NotificationCreateDTO dto, Guid userId);
}