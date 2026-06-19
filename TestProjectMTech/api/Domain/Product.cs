namespace TestProjectMTech.api.Domain;

public class Product
{
    public  int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public Status status { get; set; } = Status.Active;
}