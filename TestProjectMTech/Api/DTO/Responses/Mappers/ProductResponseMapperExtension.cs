using TestProjectMTech.Api.Domain;

namespace TestProjectMTech.Api.DTO.Responses.Mappers;

public static class ProductResponseMapperExtension
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Sku = product.Sku,
            CategoryId = product.CategoryId,
            Status = product.Status
        };
    }

    public static List<ProductResponse> ToResponse(this IEnumerable<Product> products)
    {
        return products.Select(product => product.ToResponse()).ToList();
    }
}
