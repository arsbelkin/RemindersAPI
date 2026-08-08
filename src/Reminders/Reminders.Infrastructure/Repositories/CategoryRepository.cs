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
}