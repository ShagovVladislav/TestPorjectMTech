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

    public async Task<List<Category>> GetAllCategories(CancellationToken cancellationToken)
    {
        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Select(c => c.ToDomain())
            .ToListAsync(cancellationToken);
        
        return categories;
    }

    public async Task<Category> CreateCategory(Category category, CancellationToken cancellationToken)
    {
        var savedCategory = _dbContext.Add(category.ToModel()).Entity;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return savedCategory.ToDomain();
    }
}
