using TestProjectMTech.api.Domain;

namespace TestProjectMTech.api.Data.Models.Mappers;

public static class ProductMapperExtension
{
    public static Product ToDomain(this ProductModel productModel)
    {
        return new Product
        {
            Id = productModel.Id,
            Name = productModel.Name,
            CategoryId = productModel.CategoryId,
            Status = productModel.Status,
            Sku = productModel.Sku
        };
    }

    public static ProductModel ToModel(this Product product)
    {
        return new ProductModel
        {
            Id = product.Id,
            Name = product.Name,
            CategoryId = product.CategoryId,
            Status = product.Status,
            Sku = product.Sku
        };
    }
}