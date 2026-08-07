using Reminders.Domain.Enums;

namespace Reminders.Domain.Models;

public class Notification
{
    public Guid Id { get; set; }
    
    public Guid ReminderId { get; set; }
    
    public Guid ReceiverId { get; set; }

    public ProcessedStatusTypes IsProcessed { get; set; }
    
    public DateTime NotificationTime { get; set; }
    
    public RecurrencyTypes Recurrency { get; set; }
    
    public User Receiver { get; set; }
    
    public Reminder Reminder { get; set; }
}