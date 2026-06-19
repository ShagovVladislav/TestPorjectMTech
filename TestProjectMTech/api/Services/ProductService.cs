using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;
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

    public async Task<List<Product>> GetProducts(GetProductsFilters filters)
    {
        var products = await _productRepository.GetProducts(filters);
        
        return products;
    }

    public async Task<Product?> GetProductById(int id)
    {
        var product = await _productRepository.GetProductById(id);
        
        return product;
    }

    public async Task<Product> CreateProduct(CreateProductRequest request)
    {
        if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.SKU))
            throw new Exception("Name and SKU must not be empty");
        
        var product = new Product
        {
            Name = request.Name,
            CategoryId =  request.CategoryId,
            Status = request.Status,
            Sku = request.SKU
        };
        
        return await _productRepository.CreateProduct(product);
    }

    public async Task<Product> ChangeStatus(int id, Status status)
    {
        var updatedProduct = await _productRepository.ChangeStatus(id, status);
        
        return updatedProduct;
    }
}