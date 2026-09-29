using Microsoft.EntityFrameworkCore;
using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.TransferModels.Notification;
using Reminders.Domain.Enums;
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

    public async Task<List<Notification>> GetComingNotificationsAsync()
    {
        var now =  DateTime.UtcNow;
        var nextHour =  now.AddHours(1);
        
        var res = await _dbContext.Notifications
            .Where(n => n.IsProcessed == ProcessedStatusTypes.NotProcessed && 
                        n.NotificationTime > now &&
                        n.NotificationTime <= nextHour)
            .Include(n => n.Receiver)
            .Include(n => n.Reminder)
            .ThenInclude(r => r.Category)
            .ToListAsync();

        return res;
    }

    public async Task UpdateNotificationsListAsync(List<Notification> notifications)
    {
        foreach (var notification in notifications)
        {
            _dbContext.Notifications.Update(notification);
        }
        
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<NotificationListDTO>> GetUserNotifications(Guid userId)
    {
        var res = await _dbContext.Notifications
            .Where(x => x.Receiver.Id == userId)
            .Select(x => new NotificationListDTO
            {
                Id = x.Id,
                ReminderId = x.ReminderId,
                ReminderTitle = x.Reminder.Title,
                NotificationTime = x.NotificationTime,
                Recurrency = x.Recurrency
            }).ToListAsync();

        return res;
    }

    public async Task<Notification?> GetNotification(Guid notificationId, Guid userId)
    {
        var res = await _dbContext.Notifications
            .FirstOrDefaultAsync(x => x.Id == notificationId && x.Receiver.Id == userId);

        return res;
    }

    public async Task DeleteNotification(Notification notification)
    {
        _dbContext.Notifications.Remove(notification);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }
}