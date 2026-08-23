using Reminders.Application.Exceptions;
using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels.Notification;
using Reminders.Domain.Enums;
using Reminders.Domain.Models;

namespace Reminders.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IUserRepository _userRepository;
    private readonly IReminderRepository _reminderRepository;
    private readonly INotificationRepository _notificationRepository;
    
    public NotificationService(
        IReminderRepository reminderRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository
    )
    {
        _reminderRepository = reminderRepository;
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
    }
    
    public async Task<Guid> CreateNotificationAsync(NotificationCreateDTO dto, Guid userId)
    {
        if (!(await _reminderRepository.CheckUserReminderByIdAsync(dto.ReminderId, userId)))
            throw new NotValidReminderException();

        var notification = new Notification
        {
            Id = Guid.CreateVersion7(),
            ReminderId =  dto.ReminderId,
            ReceiverId = userId,
            IsProcessed = ProcessedStatusTypes.NotProcessed,
            NotificationTime = dto.NotificationTime,
            Recurrency = dto.Recurrency
        };

        await _notificationRepository.CreateNotificationAsync(notification);
        
        return notification.Id;
    }
}