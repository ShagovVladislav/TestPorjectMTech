using Microsoft.EntityFrameworkCore;
using Npgsql;
using TestProjectMTech.Api.Data;
using TestProjectMTech.Api.Data.Models.Mappers;
using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.Exceptions;
using TestProjectMTech.Api.Repositories.Interfaces;

namespace TestProjectMTech.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbContextFactory<WarehouseDbContext> _dbContextFactory;

    public ProductRepository(IDbContextFactory<WarehouseDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<Product>> GetProducts(GetProductsFilters filters, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var products = dbContext.Products
            .AsNoTracking();
        
        if (filters.CategoryId.HasValue)
            products = products.Where(p => p.CategoryId == filters.CategoryId);

        if (filters.Status.HasValue)
            products = products.Where(p => p.Status == filters.Status);

        var skip = (filters.Page - 1) * filters.PageSize;
        
        products = products
            .OrderBy(p => p.Id)
            .Skip(skip)
            .Take(filters.PageSize);

        var result = (await products.ToListAsync(cancellationToken)).Select(p => p.ToDomain()).ToList();
        
        return result;
    }

    public async Task<Product?> GetProductById(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        return product?.ToDomain();
    }

    public async Task<Product> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var categoryExists = await dbContext.Categories.AnyAsync(c => c.Id == product.CategoryId, cancellationToken);
        
        if (!categoryExists)
            throw new NotFoundException($"Category with id {product.CategoryId} was not found");
        
        var skuExists = await dbContext.Products.AnyAsync(
            p => p.Sku == product.Sku,
            cancellationToken);
        
        if (skuExists)
            throw new ConflictException($"Product with SKU '{product.Sku}' already exists");
        
        var savedProduct = dbContext.Add(product.ToModel()).Entity;
        
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
            when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new ConflictException($"Product with SKU '{product.Sku}' already exists");
        }
        
        return savedProduct.ToDomain();
    }

    public async Task<Product> ChangeStatus(int id, Status status, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var productToUpdate = await dbContext.Products.FindAsync([id], cancellationToken);

        if (productToUpdate == null)
            throw new NotFoundException($"Product with id {id} was not found");

        productToUpdate.Status = status;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return productToUpdate.ToDomain();
    }
}
