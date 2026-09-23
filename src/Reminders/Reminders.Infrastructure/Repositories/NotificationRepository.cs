using Microsoft.EntityFrameworkCore;
using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.TransferModels.Notification;
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

    public async Task<List<NotificationMessageDTO>> GetComingNotificationsAsync(CancellationToken stoppingToken)
    {
        var res = await _dbContext.Notifications
            .Where(n => n.NotificationTime > DateTime.UtcNow &&
                        n.NotificationTime <= DateTime.UtcNow.AddHours(1))
            .Select(x => new NotificationMessageDTO
            {
                Id = x.Id,
                Title = x.Reminder.Title
            }).ToListAsync(stoppingToken);

        return res;
    }
}