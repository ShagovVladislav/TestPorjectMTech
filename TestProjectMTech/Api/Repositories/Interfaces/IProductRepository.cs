using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.DTO.Responses;

namespace TestProjectMTech.Api.Repositories.Interfaces;

public interface IProductRepository
{
    Task<PagedResult<Product>> GetProducts(GetProductsFilters filters, CancellationToken cancellationToken);
    Task<Product?> GetProductById(int id, CancellationToken cancellationToken);
    Task<Product> CreateProduct(Product product, CancellationToken cancellationToken);
    Task<Product> ChangeStatus(int id, Status status, CancellationToken cancellationToken);
    Task<bool> ExistsBySku(string sku, CancellationToken cancellationToken);

}
