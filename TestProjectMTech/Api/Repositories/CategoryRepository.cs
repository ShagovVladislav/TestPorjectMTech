using Microsoft.EntityFrameworkCore;
using TestProjectMTech.Api.Data;
using TestProjectMTech.Api.Data.Models.Mappers;
using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.Repositories.Interfaces;

namespace TestProjectMTech.Api.Repositories;

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

    public async Task<Category?> GetCategoryById(int id, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        return category?.ToDomain();
    }

    public async Task<Category> CreateCategory(Category category, CancellationToken cancellationToken)
    {
        var savedCategory = _dbContext.Add(category.ToModel()).Entity;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return savedCategory.ToDomain();
    }
}
