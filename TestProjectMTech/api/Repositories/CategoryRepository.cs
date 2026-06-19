using Microsoft.EntityFrameworkCore;
using TestProjectMTech.api.Data;
using TestProjectMTech.api.Data.Models.Mappers;
using TestProjectMTech.api.Domain;
using TestProjectMTech.api.Repositories.Interfaces;

namespace TestProjectMTech.api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly WarehouseDbContext _dbContext;

    public CategoryRepository(WarehouseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        var categories = await _dbContext.Categories.Select(c => c.ToDomain()).ToListAsync();
        
        return categories;
    }

    public async Task<Category> CreateCategory(Category category)
    {
        var savedCategory = _dbContext.Add(category.ToModel()).Entity;
        
        await _dbContext.SaveChangesAsync();
        
        return savedCategory.ToDomain();
    }
}
