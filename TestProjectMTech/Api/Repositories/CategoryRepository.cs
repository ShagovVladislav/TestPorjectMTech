using Microsoft.EntityFrameworkCore;
using TestProjectMTech.Api.Data;
using TestProjectMTech.Api.Data.Models.Mappers;
using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.Repositories.Interfaces;

namespace TestProjectMTech.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbContextFactory<WarehouseDbContext> _dbContextFactory;

    public CategoryRepository(IDbContextFactory<WarehouseDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<Category>> GetAllCategories(CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var categories = await dbContext.Categories
            .AsNoTracking()
            .Select(c => c.ToDomain())
            .ToListAsync(cancellationToken);
        
        return categories;
    }

    public async Task<Category?> GetCategoryById(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var category = await dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        return category?.ToDomain();
    }

    public async Task<Category> CreateCategory(Category category, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var savedCategory = dbContext.Add(category.ToModel()).Entity;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return savedCategory.ToDomain();
    }
}
