using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;

namespace TestProjectMTech.Api.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetProducts(GetProductsFilters filters, CancellationToken cancellationToken);
    Task<Product> GetProductById(int id, CancellationToken cancellationToken);
    Task<Product> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken);
    Task<Product> ChangeStatus(int id, Status status, CancellationToken cancellationToken);
}
