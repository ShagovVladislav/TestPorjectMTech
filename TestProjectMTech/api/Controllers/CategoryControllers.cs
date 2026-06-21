using Microsoft.AspNetCore.Mvc;
using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;
using TestProjectMTech.api.Services.Interfaces;

namespace TestProjectMTech.api.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryControllers : ControllerBase
{
    private readonly ICategoryService _categoryService;
    
    public CategoryControllers(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllCategories(cancellationToken);
        
        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(CreateCategoryRequest category, CancellationToken cancellationToken)
    {
        var categoryResponse = await _categoryService.CreateCategory(category, cancellationToken);
        
        return StatusCode(StatusCodes.Status201Created, categoryResponse);
    }
}
