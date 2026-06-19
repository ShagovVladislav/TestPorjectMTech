using TestProjectMTech.api.Domain;

namespace TestProjectMTech.api.Data.Models.Mappers;

public static class CategoryMapperExtension
{
    public static Category ToDomain(this CategoryModel categoryModel)
    {
        return new Category
        {
            Id = categoryModel.Id,
            Name = categoryModel.Name,
        };
    }

    public static CategoryModel ToModel(this Category category)
    {
        return new CategoryModel
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}