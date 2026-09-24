using Microsoft.EntityFrameworkCore;
using Reminders.Application.Enums;
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

    public async Task<List<NotificationMessageDTO>> GetComingNotificationsAsync(CancellationToken stoppingToken)
    {
        var res = await _dbContext.Notifications
            .Where(n => n.IsProcessed == ProcessedStatusTypes.NotProcessed)
            .Where(n => n.NotificationTime > DateTime.UtcNow &&
                        n.NotificationTime <= DateTime.UtcNow.AddHours(1))
            .Select(x => new NotificationMessageDTO
            {
                MessageType = MessageTypes.Add,
                NotificationId = x.Id,
                ReceiverEmail = x.Receiver.Email,
                CategoryTitle = x.Reminder.Category.Title,
                ReminderTitle = x.Reminder.Title,
                ReminderDescription = x.Reminder.Description,
                NotificationTime = x.NotificationTime
            }).ToListAsync(stoppingToken);

        return res;
    }

    public async Task UpdateComingNotificationsAsync(List<NotificationMessageDTO> notifications,
        CancellationToken stoppingToken)
    {
        await _dbContext.Notifications
            .Where(n => notifications.Select(x => x.NotificationId).Contains(n.Id))
            .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(x => x.IsProcessed, ProcessedStatusTypes.Processed),
                stoppingToken);
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
}