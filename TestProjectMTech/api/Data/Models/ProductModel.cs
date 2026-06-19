using TestProjectMTech.api.Domain;

namespace TestProjectMTech.api.Data.Models;

public class ProductModel
{
    public  int Id { get; set; }
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public int CategoryId { get; set; }
    public CategoryModel Category { get; set; } = null!;
    public Status Status { get; set; } = Status.Active;

    public void ChangeStatus(Status newStatus)
    {
        var transitionAllowed = (Status, newStatus) switch
        {
            (Status.Active, Status.Defective) => true,
            (Status.Defective, Status.WriteOff) => true,
            _ => false
        };
        
        if (!transitionAllowed)
            throw new Exception("can't change status");
        
        Status = newStatus;
    }
}