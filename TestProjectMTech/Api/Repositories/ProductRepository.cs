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
    private readonly WarehouseDbContext _dbContext;

    public ProductRepository(WarehouseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetProducts(GetProductsFilters filters, CancellationToken cancellationToken)
    {
        var products = _dbContext.Products
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

        productToUpdate.Status = status;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return productToUpdate.ToDomain();
    }
}
