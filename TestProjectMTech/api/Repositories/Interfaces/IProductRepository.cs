using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;

namespace TestProjectMTech.api.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetProducts(GetProductsFilters filters);
    Task<Product?> GetProductById(int id);
    Task<Product> CreateProduct(Product product);
    Task<Product> ChangeStatus(int id, Status status);
}