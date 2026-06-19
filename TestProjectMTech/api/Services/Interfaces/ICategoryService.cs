using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;

namespace TestProjectMTech.api.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategories();
    Task<Category> CreateCategory(CreateCategoryRequest categoryRequest);
}