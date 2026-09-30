using Reminders.Application.Enums;
using Reminders.Application.Exceptions;
using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels.Notification;
using Reminders.Domain.Enums;
using Reminders.Domain.Models;

namespace Reminders.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IReminderRepository _reminderRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IRedisNotification _redis;

    public NotificationService(
        IReminderRepository reminderRepository,
        INotificationRepository notificationRepository,
        IRedisNotification redis
    )
    {
        _reminderRepository = reminderRepository;
        _notificationRepository = notificationRepository;
        _redis = redis;
    }

    public async Task<Guid> CreateNotificationAsync(NotificationCreateDTO dto, Guid userId)
    {
        if (!(await _reminderRepository.CheckUserReminderByIdAsync(dto.ReminderId, userId)))
            throw new NotValidReminderException();

        var notification = new Notification
        {
            Id = Guid.CreateVersion7(),
            ReminderId = dto.ReminderId,
            ReceiverId = userId,
            IsProcessed = ProcessedStatusTypes.NotProcessed,
            NotificationTime = dto.NotificationTime,
            Recurrency = dto.Recurrency
        };

        await _notificationRepository.CreateNotificationAsync(notification);

        return notification.Id;
    }

    public async Task<List<NotificationListDTO>> GetUserNotificationsAsync(Guid userId)
    {
        var notifications = await _notificationRepository.GetUserNotificationsAsync(userId);

        return notifications;
    }

    public async Task DeleteNotificationAsync(Guid notificationId, Guid userId)
    {
        var notification = await _notificationRepository.GetNotificationAsync(notificationId, userId);
        
        if (notification == null)
            throw new NotValidReminderException();

        await _redis.ProcessMessageAsync(new NotificationWrapper
        {
            NotificationId = notification.Id,
            MessageType = MessageTypes.Add
        });
        
        await _notificationRepository.DeleteNotificationAsync(notification);
    }
}