using Reminders.Application.TransferModels;

namespace Reminders.Application.Services.Interfaces;

public interface ICategoryService
{
    public Task<Guid> CreateCategoryAsync(CategoryCreateDTO dto);
}