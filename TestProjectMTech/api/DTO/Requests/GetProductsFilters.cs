using TestProjectMTech.api.Domain;

namespace TestProjectMTech.api.DTO.Requests;

public class GetProductsFilters
{
    public int? categoryId { get; set; } = null;
    public Status? status { get; set; } = null;
}