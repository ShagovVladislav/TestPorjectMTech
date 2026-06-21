namespace TestProjectMTech.Api.Data.Models;

public class CategoryModel
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    
    public List<ProductModel> Products { get; set; } = [];
}