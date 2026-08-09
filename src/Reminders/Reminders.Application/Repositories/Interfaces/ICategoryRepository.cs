using Reminders.Domain.Models;

namespace Reminders.Application.Repositories.Interfaces;

public interface ICategoryRepository
{
    public Task CreateCategoryAsync(Category category);
    public Task<List<Category>> GetUserCategoriesAsync(Guid userId);
    public Task<Category?> GetCategoryByIdAsync(Guid categoryId);
    public Task UpdateCategoryAsync(Category category);
    public Task  DeleteCategoryAsync(Category category);
    public Task<bool> CheckCategoryByIdAsync(Guid categoryId);
}