using System.ComponentModel.DataAnnotations;
using TestProjectMTech.api.Constants;

namespace TestProjectMTech.api.DTO.Requests;

public class CreateProductRequest
{
    [Required]
    [MaxLength(RequestStringConstants.ProductNameMaxLength)]
    [MinLength(RequestStringConstants.ProductNameMinLength)]
    public required string Name { get; set; }
    
    [Required]
    [MaxLength(RequestStringConstants.SkuNameMaxLength)]
    [MinLength(RequestStringConstants.SkuNameMinLength)]
    public required string SKU { get; set; }
    
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}
