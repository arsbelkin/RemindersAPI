using Microsoft.EntityFrameworkCore;
using Reminders.Application.Repositories.Interfaces;
using Reminders.Domain.Models;
using Reminders.Infrastructure.Contexts;

namespace Reminders.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly RemindersContext _dbContext;
    
    public CategoryRepository(RemindersContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateCategoryAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Category>> GetUserCategoriesAsync(Guid userId)
    {
        var categories = await _dbContext.Categories
            .Where(c => c.CreatorId == userId)
            .ToListAsync();
        
        return categories;
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
    {
        var cat = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        return cat;
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(Category category)
    {
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> CheckCategoryByIdAsync(Guid categoryId)
    {
        return await _dbContext.Categories.AnyAsync(c => c.Id == categoryId);
    }
}