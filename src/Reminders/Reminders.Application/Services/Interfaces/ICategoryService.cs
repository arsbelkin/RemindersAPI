using Reminders.Application.TransferModels.Category;

namespace Reminders.Application.Services.Interfaces;

public interface ICategoryService
{
    public Task<Guid> CreateCategoryAsync(CategoryCreateDTO dto);
    public Task<List<CategoryViewDTO>> GetUserCategoriesAsync(Guid userId);
}