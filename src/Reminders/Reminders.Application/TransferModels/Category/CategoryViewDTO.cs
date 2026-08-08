namespace Reminders.Application.TransferModels.Category;

public class CategoryViewDTO
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedTime { get; set; }
}