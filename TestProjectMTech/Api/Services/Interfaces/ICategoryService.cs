using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;

namespace TestProjectMTech.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategories(CancellationToken cancellationToken);
    Task<Category> GetCategoryById(int id, CancellationToken cancellationToken);
    Task<Category> CreateCategory(CreateCategoryRequest categoryRequest, CancellationToken cancellationToken);
}
