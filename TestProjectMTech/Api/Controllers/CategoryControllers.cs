using Microsoft.AspNetCore.Mvc;
using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.Services.Interfaces;

namespace TestProjectMTech.Api.Controllers;

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
        
        return Created("api/categories", categoryResponse);
    }
}
