using Microsoft.EntityFrameworkCore;
using Npgsql;
using TestProjectMTech.Api.Data;
using TestProjectMTech.Api.Data.Models.Mappers;
using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.DTO.Responses;
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

    public async Task<PagedResult<Product>> GetProducts(GetProductsFilters filters, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var products = dbContext.Products
            .AsNoTracking();
        
        if (filters.CategoryId.HasValue)
            products = products.Where(p => p.CategoryId == filters.CategoryId);

        if (filters.Status.HasValue)
            products = products.Where(p => p.Status == filters.Status);

        var totalCount = await products.CountAsync(cancellationToken);
        
        var skip = (filters.Page - 1) * filters.PageSize;
        
        products = products
            .OrderBy(p => p.Id)
            .Skip(skip)
            .Take(filters.PageSize);

        var result = (await products.ToListAsync(cancellationToken)).Select(p => p.ToDomain()).ToList();
        
        return new PagedResult<Product>
        {
            Items = result,
            TotalCount = totalCount,
            Page = filters.Page,
            PageSize = filters.PageSize
        };
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

    public async Task<bool> ExistsBySku(string sku, CancellationToken cancellationToken)
    {
        await using var dbContext =
            await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Products
            .AsNoTracking()
            .AnyAsync(product => product.Sku == sku, cancellationToken);
    }
}
