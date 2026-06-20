using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;

namespace TestProjectMTech.api.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategories(CancellationToken cancellationToken);
    Task<Category> CreateCategory(CreateCategoryRequest categoryRequest, CancellationToken cancellationToken);
}
