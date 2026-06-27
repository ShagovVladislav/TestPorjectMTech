using TestProjectMTech.Api.Domain;

namespace TestProjectMTech.Api.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategories(CancellationToken cancellationToken);
    Task<Category?> GetCategoryById(int id, CancellationToken cancellationToken);
    Task<Category> CreateCategory(Category category, CancellationToken cancellationToken);
    Task<bool> ExistsById(int id, CancellationToken cancellationToken);
}
