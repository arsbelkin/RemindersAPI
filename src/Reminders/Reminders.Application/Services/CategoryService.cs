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
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
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

    public async Task<List<CategoryViewDTO>> GetUserCategoriesAsync(Guid userId)
    {
        var userCategories = await _categoryRepository.GetUserCategoriesAsync(userId);
        
        return _mapper.Map<List<CategoryViewDTO>>(userCategories);
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