using TestProjectMTech.Api.Domain;

namespace TestProjectMTech.Api.DTO.Responses.Mappers;

public static class CategoryResponseMapperExtension
{
    public static CategoryResponse ToResponse(this Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public static List<CategoryResponse> ToResponse(this IEnumerable<Category> categories)
    {
        return categories.Select(category => category.ToResponse()).ToList();
    }
}
