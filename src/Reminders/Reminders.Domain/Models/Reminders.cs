using Reminders.Domain.Enums;

namespace Reminders.Domain.Models;

public class Reminders
{
    public Reminders()
    {
    }
    
    public Guid Id { get; set; }
    
    public Guid CreatorId { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedTime { get; set; }
    
    public PriorityTypes Priority { get; set; }
    
    public CompletedStatusTypes IsCompleted { get; set; }
    
    public DateTime? CompletedTime { get; set; }
    
    public RecurrencyTypes RecurrencyType  { get; set; }
    
    public DateTime? RecurrencyDateTime { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public Users Creator { get; set; }
    
    public Categories Category { get; set; }
}