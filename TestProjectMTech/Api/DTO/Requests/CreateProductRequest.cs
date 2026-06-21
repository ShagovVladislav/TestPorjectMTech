using System.ComponentModel.DataAnnotations;
using TestProjectMTech.Api.Constants;

namespace TestProjectMTech.Api.DTO.Requests;

public class CreateProductRequest
{
    [Required]
    [MaxLength(RequestStringConstants.ProductNameMaxLength)]
    [MinLength(RequestStringConstants.ProductNameMinLength)]
    public required string Name { get; set; }
    
    [Required]
    [MaxLength(RequestStringConstants.SkuNameMaxLength)]
    [MinLength(RequestStringConstants.SkuNameMinLength)]
    public required string Sku { get; set; }
    
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}
