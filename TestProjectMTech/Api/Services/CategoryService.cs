using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.Exceptions;
using TestProjectMTech.Api.Repositories.Interfaces;
using TestProjectMTech.Api.Services.Interfaces;

namespace TestProjectMTech.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> GetAllCategories(CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllCategories(cancellationToken);

        return categories;
    }

    public async Task<Category> GetCategoryById(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetCategoryById(id, cancellationToken);

        return category ?? throw new NotFoundException($"Category with id {id} was not found");
    }

    public async Task<Category> CreateCategory(CreateCategoryRequest categoryRequest, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            throw new ValidationException("Category name must not be empty");
        
        var category = new Category{ Name = categoryRequest.Name };
        var result = await _categoryRepository.CreateCategory(category, cancellationToken);
        
        return result;
    }
}
