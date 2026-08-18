namespace Reminders.Application.TransferModels.Category;

public class CategoryInfoViewDTO
{
    public Guid Id { get; set; }
    
    public Guid CreatorId { get; set; }
    
    public string? CreatorEmail { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedTime { get; set; }
}