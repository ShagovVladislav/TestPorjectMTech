using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.DTO.Responses;
using TestProjectMTech.Api.DTO.Responses.Mappers;
using TestProjectMTech.Api.Exceptions;
using TestProjectMTech.Api.Repositories.Interfaces;
using TestProjectMTech.Api.Services.Interfaces;
using TestProjectMTech.Api.Services.Policies;

namespace TestProjectMTech.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductStatusPolicy _productStatusPolicy;

    public ProductService(
        IProductRepository productRepository,
        IProductStatusPolicy productStatusPolicy, 
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _productStatusPolicy = productStatusPolicy;
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResult<ProductResponse>> GetProducts(GetProductsFilters filters, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProducts(filters, cancellationToken);

        var result = new PagedResult<ProductResponse>
        {
            Items = products.Items.Select(p => p.ToResponse()).ToList(),
            TotalCount = products.TotalCount,
            Page =  products.Page,
            PageSize = products.PageSize
        };

        return result;
    }

    public async Task<Product> GetProductById(int id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductById(id, cancellationToken);
        
        return product ?? throw new NotFoundException($"Product with id {id} was not found");
    }

    public async Task<Product> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var categoryExist = await _categoryRepository.ExistsById(request.CategoryId, cancellationToken);
        
        if (!categoryExist)
            throw new NotFoundException($"Category with id {request.CategoryId} was not found");
            
        var skuExist = await _productRepository.ExistsBySku(request.Sku, cancellationToken);
        
        if (skuExist)
            throw new ConflictException($"Product with SKU '{request.Sku}' already exists");
        
        var product = new Product
        {
            Name = request.Name,
            CategoryId = request.CategoryId,
            Status = Status.Active,
            Sku = request.Sku
        };
        
        return await _productRepository.CreateProduct(product, cancellationToken);
    }

    public async Task<Product> ChangeStatus(int id, Status status, CancellationToken cancellationToken)
    {
        var product = await GetProductById(id, cancellationToken);
        _productStatusPolicy.EnsureCanChangeStatus(product.Status, status);
        
        var updatedProduct = await _productRepository.ChangeStatus(id, status, cancellationToken);
        
        return updatedProduct;
    }
}
