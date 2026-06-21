using Microsoft.AspNetCore.Mvc;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.DTO.Responses;
using TestProjectMTech.Api.DTO.Responses.Mappers;
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
    public async Task<ActionResult<List<CategoryResponse>>> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllCategories(cancellationToken);
        
        return Ok(categories.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> CreateCategory(
        CreateCategoryRequest category,
        CancellationToken cancellationToken)
    {
        var createdCategory = await _categoryService.CreateCategory(category, cancellationToken);
        
        return Created("api/categories", createdCategory.ToResponse());
    }
}
