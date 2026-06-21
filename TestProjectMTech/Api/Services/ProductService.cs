using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.Exceptions;
using TestProjectMTech.Api.Repositories.Interfaces;
using TestProjectMTech.Api.Services.Interfaces;
using TestProjectMTech.Api.Services.Policies;

namespace TestProjectMTech.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductStatusPolicy _productStatusPolicy;

    public ProductService(
        IProductRepository productRepository,
        IProductStatusPolicy productStatusPolicy)
    {
        _productRepository = productRepository;
        _productStatusPolicy = productStatusPolicy;
    }

    public async Task<List<Product>> GetProducts(GetProductsFilters filters, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProducts(filters, cancellationToken);
        
        return products;
    }

    public async Task<Product> GetProductById(int id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductById(id, cancellationToken);
        
        return product ?? throw new NotFoundException($"Product with id {id} was not found");
    }

    public async Task<Product> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Sku))
            throw new ValidationException("Name and SKU must not be empty");
        
        if (request.CategoryId <= 0)
            throw new ValidationException("CategoryId must be greater than zero");
        
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
