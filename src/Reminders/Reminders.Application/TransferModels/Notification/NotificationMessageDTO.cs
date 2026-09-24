using Reminders.Application.Enums;

namespace Reminders.Application.TransferModels.Notification;

public class NotificationMessageDTO
{
    public MessageTypes MessageType { get; set; }
    
    public Guid NotificationId { get; set; }
    
    public string ReceiverEmail { get; set; }
    
    public string CategoryTitle { get; set; }
    
    public string ReminderTitle { get; set; }
    
    public string? ReminderDescription { get; set; }
    
    public DateTime NotificationTime { get; set; }
}