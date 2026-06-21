using TestProjectMTech.Api.Domain;

namespace TestProjectMTech.Api.Data.Models;

public class ProductModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public int CategoryId { get; set; }
    public CategoryModel Category { get; set; } = null!;
    public Status Status { get; set; } = Status.Active;
}
