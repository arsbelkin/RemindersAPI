using Reminders.Domain.Models;

namespace Reminders.Application.Repositories.Interfaces;

public interface ICategoryRepository
{
    public Task CreateCategoryAsync(Category category);
}