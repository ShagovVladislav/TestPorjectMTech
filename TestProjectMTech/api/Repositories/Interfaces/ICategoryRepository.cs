using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;

namespace TestProjectMTech.api.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategories(CancellationToken cancellationToken);
    Task<Category> CreateCategory(Category category, CancellationToken cancellationToken);
}
