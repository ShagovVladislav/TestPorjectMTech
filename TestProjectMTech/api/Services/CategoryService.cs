using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;
using TestProjectMTech.api.Exceptions;
using TestProjectMTech.api.Repositories.Interfaces;
using TestProjectMTech.api.Services.Interfaces;

namespace TestProjectMTech.api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllCategories();

        return categories;
    }

    public async Task<Category> CreateCategory(CreateCategoryRequest categoryRequest)
    {
        if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            throw new ValidationException("Category name must not be empty");
        
        var category = new Category{ Name = categoryRequest.Name };
        var result = await _categoryRepository.CreateCategory(category);
        
        return result;
    }
}
