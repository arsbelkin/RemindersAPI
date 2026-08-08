namespace Reminders.Application.TransferModels;

public class CategoryViewDTO
{
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedTime { get; set; }
}