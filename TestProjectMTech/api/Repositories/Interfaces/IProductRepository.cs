using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;

namespace TestProjectMTech.api.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetProducts(GetProductsFilters filters, CancellationToken cancellationToken);
    Task<Product?> GetProductById(int id, CancellationToken cancellationToken);
    Task<Product> CreateProduct(Product product, CancellationToken cancellationToken);
    Task<Product> ChangeStatus(int id, Status status, CancellationToken cancellationToken);
}
