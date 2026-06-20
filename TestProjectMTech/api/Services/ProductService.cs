using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;
using TestProjectMTech.api.Exceptions;
using TestProjectMTech.api.Repositories.Interfaces;
using TestProjectMTech.api.Services.Interfaces;

namespace TestProjectMTech.api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
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
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.SKU))
            throw new ValidationException("Name and SKU must not be empty");
        
        if (request.CategoryId <= 0)
            throw new ValidationException("CategoryId must be greater than zero");
        
        var product = new Product
        {
            Name = request.Name,
            CategoryId =  request.CategoryId,
            Status = Status.Active,
            Sku = request.SKU
        };
        
        return await _productRepository.CreateProduct(product, cancellationToken);
    }

    public async Task<Product> ChangeStatus(int id, Status status, CancellationToken cancellationToken)
    {
        var updatedProduct = await _productRepository.ChangeStatus(id, status, cancellationToken);
        
        return updatedProduct;
    }
}
