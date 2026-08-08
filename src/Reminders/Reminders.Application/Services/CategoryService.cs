using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels;
using Reminders.Domain.Models;

namespace Reminders.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Guid> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        var category = new Category
        {
            Id = Guid.CreateVersion7(),
            CreatorId = dto.CreatorId,
            Title = dto.Title,
            Description = dto.Description,
            CreatedTime = DateTime.UtcNow
        };
        
        await _categoryRepository.CreateCategoryAsync(category);
        return category.Id;
    }
}