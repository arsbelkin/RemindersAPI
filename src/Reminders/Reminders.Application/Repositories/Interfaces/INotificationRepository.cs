using Reminders.Domain.Models;

namespace Reminders.Application.Repositories.Interfaces;

public interface INotificationRepository
{
    public Task CreateNotificationAsync(Notification notification);
}