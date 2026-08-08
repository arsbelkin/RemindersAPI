using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reminders.Application.Common.Converters;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels;

namespace Reminders.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly CategoriesConverter _categoriesConverter;
    
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
        _categoriesConverter = new CategoriesConverter();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CategoryRequestDTO dto)
    {
        var createDto = _categoriesConverter.ToModel(dto);
        createDto.CreatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var categoryId = await _categoryService.CreateCategoryAsync(createDto);
        return Ok(new {CategoryId = categoryId});
    }
}