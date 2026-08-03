namespace Reminders.Domain.Models;

public class Categories
{
    public Categories()
    {
    }
    
    public Guid Id { get; set; }
    
    public Guid CreatorId { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedTime { get; set; }
    
    public virtual Users Creator { get; set; }
    
    public virtual ICollection<Users> Members { get; set; } = new List<Users>();
    
    public virtual ICollection<Reminders> Reminders { get; set; } = new List<Reminders>();
}