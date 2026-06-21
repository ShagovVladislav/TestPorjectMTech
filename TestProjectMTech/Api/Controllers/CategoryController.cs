using Microsoft.AspNetCore.Mvc;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.DTO.Responses;
using TestProjectMTech.Api.DTO.Responses.Mappers;
using TestProjectMTech.Api.Services.Interfaces;

namespace TestProjectMTech.Api.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllCategories(cancellationToken);
        
        return Ok(categories.ToResponse());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponse>> GetCategoryById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var category = await _categoryService.GetCategoryById(id, cancellationToken);

        return Ok(category.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> CreateCategory(
        CreateCategoryRequest category,
        CancellationToken cancellationToken)
    {
        var createdCategory = await _categoryService.CreateCategory(category, cancellationToken);
        
        return CreatedAtAction(
            nameof(GetCategoryById),
            new { id = createdCategory.Id },
            createdCategory.ToResponse());
    }
}
