using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;

namespace TestProjectMTech.api.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetProducts(GetProductsFilters filters);
    Task<Product> GetProductById(int id);
    Task<Product> CreateProduct(CreateProductRequest request);
    Task<Product> ChangeStatus(int id, Status status);
}
