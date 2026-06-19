using TestProjectMTech.api.Domain;

namespace TestProjectMTech.api.DTO.Requests;

public class CreateProductRequest
{
    public required string Name { get; set; }
    public required string SKU { get; set; }
    public int CategoryId { get; set; }
    public Status Status { get; set; } = Status.Active;
}