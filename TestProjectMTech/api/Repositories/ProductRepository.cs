using Microsoft.EntityFrameworkCore;
using Npgsql;
using TestProjectMTech.api.Data;
using TestProjectMTech.api.Data.Models.Mappers;
using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;
using TestProjectMTech.api.Exceptions;
using TestProjectMTech.api.Repositories.Interfaces;

namespace TestProjectMTech.api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WarehouseDbContext _dbContext;

    public ProductRepository(WarehouseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetProducts(GetProductsFilters filters, CancellationToken cancellationToken)
    {
        var products = _dbContext.Products
            .AsNoTracking();
        
        if (filters.categoryId.HasValue)
            products = products.Where(p => p.CategoryId == filters.categoryId);

        if (filters.status.HasValue)
            products = products.Where(p => p.Status == filters.status);

        var skip = (filters.page - 1) * filters.pageSize;
        
        products = products
            .OrderBy(p => p.Id)
            .Skip(skip)
            .Take(filters.pageSize);

        var result = (await products.ToListAsync(cancellationToken)).Select(p => p.ToDomain()).ToList();
        
        return result;
    }

    public async Task<Product?> GetProductById(int id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        return product?.ToDomain();
    }

    public async Task<Product> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        var categoryExists = await _dbContext.Categories.AnyAsync(c => c.Id == product.CategoryId, cancellationToken);
        
        if (!categoryExists)
            throw new NotFoundException($"Category with id {product.CategoryId} was not found");
        
        var skuExists = await _dbContext.Products.AnyAsync(
            p => p.Sku == product.Sku,
            cancellationToken);
        
        if (skuExists)
            throw new ConflictException($"Product with SKU '{product.Sku}' already exists");
        
        var savedProduct = _dbContext.Add(product.ToModel()).Entity;
        
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
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
        var productToUpdate = await _dbContext.Products.FindAsync([id], cancellationToken);

        if (productToUpdate == null)
            throw new NotFoundException($"Product with id {id} was not found");
        
        if (!productToUpdate.CanChangeStatus(status))
            throw new InvalidStatusTransitionException(productToUpdate.Status, status);
        
        productToUpdate.ChangeStatus(status);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return productToUpdate.ToDomain();
    }
}
