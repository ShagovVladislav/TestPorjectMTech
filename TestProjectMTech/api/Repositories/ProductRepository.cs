using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Product>> GetProducts(GetProductsFilters filters)
    {
        var products = _dbContext.Products
            .AsNoTracking();
        
        if (filters.categoryId.HasValue)
            products = products.Where(p => p.CategoryId ==  filters.categoryId);

        if (filters.status.HasValue)
            products = products.Where(p => p.Status == filters.status);

        var result = (await products.ToListAsync()).Select(p => p.ToDomain()).ToList();
        
        return result;
    }

    public async Task<Product?> GetProductById(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        
        return product?.ToDomain();
    }

    public async Task<Product> CreateProduct(Product product)
    {
        var categoryExists = await _dbContext.Categories.AnyAsync(c => c.Id == product.CategoryId);
        if (!categoryExists)
            throw new NotFoundException($"Category with id {product.CategoryId} was not found");
        
        var skuExists = await _dbContext.Products.AnyAsync(p => p.Sku == product.Sku);
        if (skuExists)
            throw new ConflictException($"Product with SKU '{product.Sku}' already exists");
        
        var savedProduct = (await _dbContext.AddAsync(product.ToModel())).Entity;
        
        await _dbContext.SaveChangesAsync();
        return savedProduct.ToDomain();
    }

    public async Task<Product> ChangeStatus(int id, Status status)
    {
        var productToUpdate = await _dbContext.Products.FindAsync(id);

        if (productToUpdate == null)
            throw new NotFoundException($"Product with id {id} was not found");
        
        productToUpdate.ChangeStatus(status);
        
        await _dbContext.SaveChangesAsync();
        return productToUpdate.ToDomain();
    }
}
