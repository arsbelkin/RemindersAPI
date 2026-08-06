using Reminders.Domain.Enums;

namespace Reminders.Domain.Models;

public class Notifications
{
    public Guid Id { get; set; }
    
    public Guid ReminderId { get; set; }
    
    public Guid ReceiverId { get; set; }

    public ProcessedStatusTypes IsProcessed { get; set; }
    
    public DateTime NotificationTime { get; set; }
    
    public RecurrencyTypes Recurrency { get; set; }
    
    public Users Receiver { get; set; }
    
    public Reminders Reminder { get; set; }
}