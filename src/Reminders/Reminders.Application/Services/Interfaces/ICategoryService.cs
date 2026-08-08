using Reminders.Application.TransferModels.Category;
using Reminders.Domain.Models;

namespace Reminders.Application.Services.Interfaces;

public interface ICategoryService
{
    public Task<Guid> CreateCategoryAsync(CategoryCreateDTO dto);
    public Task<List<CategoryViewDTO>> GetUserCategoriesAsync(Guid userId);
    public Task UpdateCategoryAsync(CategoryUpdateDTO dto);
}