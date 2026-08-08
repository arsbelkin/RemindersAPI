namespace Reminders.Application.TransferModels.Category;

public class CategoryUpdateDTO
{
    public Guid Id { get; set; }
    
    public Guid CreatorId { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
}