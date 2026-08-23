using Reminders.Application.Repositories.Interfaces;
using Reminders.Domain.Models;
using Reminders.Infrastructure.Contexts;

namespace Reminders.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly RemindersContext _dbContext;

    public NotificationRepository(RemindersContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateNotificationAsync(Notification notification)
    {
        await _dbContext.Notifications.AddAsync(notification);
        await _dbContext.SaveChangesAsync();
    }
}