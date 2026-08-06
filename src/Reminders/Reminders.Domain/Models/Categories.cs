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
    
    public Users Creator { get; set; }
}