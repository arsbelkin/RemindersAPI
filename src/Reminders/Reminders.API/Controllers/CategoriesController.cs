using System.Security.Claims;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels;

namespace Reminders.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    
    public CategoriesController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CategoryRequestDTO dto)
    {
        var createDto = _mapper.Map<CategoryCreateDTO>(dto);
        createDto.CreatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var categoryId = await _categoryService.CreateCategoryAsync(createDto);
        return Ok(new {CategoryId = categoryId});
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryViewDTO>>> GetUserCategories()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var userCategories = await _categoryService.GetUserCategoriesAsync(userId);
        
        return Ok(userCategories);
    }
}