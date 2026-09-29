using MapsterMapper;
using Reminders.Application.Exceptions;
using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels.Category;
using Reminders.Domain.Models;

namespace Reminders.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IReminderRepository _reminderRepository;
    private readonly IMapper _mapper;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IReminderRepository reminderRepository,
        IMapper mapper
    )
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _reminderRepository = reminderRepository;
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

    public async Task<List<CategoryListViewDTO>> GetUserCategoriesAsync(
        Guid userId,
        string? categoryName,
        Guid? categoryId
    )
    {
        var userRemindersQuery = _reminderRepository.GetRemindersByUserIdQuery(userId);
        var userCategoriesQuery = _categoryRepository.CreateCategoryQuery(categoryName, categoryId);

        var userCategories = await _categoryRepository.GetUserCategoriesAsync(
            userRemindersQuery,
            userCategoriesQuery,
            userId);

        return _mapper.Map<List<CategoryListViewDTO>>(userCategories);
    }

    public async Task<CategoryInfoViewDTO> GetCategoryInfoAsync(Guid categoryId, Guid userId)
    {
        var userRemindersQuery = _reminderRepository.GetRemindersByUserIdQuery(userId);

        var categoryInfo = await _categoryRepository.GetCategoryInfoAsync(userId, categoryId, userRemindersQuery);

        if (categoryInfo == null)
            throw new NotValidCategoryException();

        return categoryInfo;
    }

    public async Task UpdateCategoryAsync(CategoryUpdateDTO dto)
    {
        var cat = await _categoryRepository.GetCategoryByIdAsync(dto.Id);

        if (cat == null)
            throw new NotValidCategoryException();

        if (dto.CreatorId != cat.CreatorId)
            throw new NotValidCategoryException();

        if (dto.Title is not null)
            cat.Title = dto.Title;

        cat.Description = dto.Description;

        await _categoryRepository.UpdateCategoryAsync(cat);
    }

    public async Task DeleteCategoryAsync(CategoryDeleteDTO dto)
    {
        var cat = await _categoryRepository.GetCategoryByIdAsync(dto.Id);

        if (cat == null)
            throw new NotValidCategoryException();

        if (dto.CreatorId != cat.CreatorId)
            throw new NotValidCategoryException();

        await _categoryRepository.DeleteCategoryAsync(cat);
    }
}