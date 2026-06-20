using System.ComponentModel.DataAnnotations;
using TestProjectMTech.api.Constants;

namespace TestProjectMTech.api.DTO.Requests;

public class CreateProductRequest
{
    [MaxLength(RequestStringConstants.ProductNameMaxLength)]
    [MinLength(RequestStringConstants.ProductNameMinLength)]
    public required string Name { get; set; }
    
    [MaxLength(RequestStringConstants.SkuNameMaxLength)]
    [MinLength(RequestStringConstants.SkuNameMinLength)]
    public required string SKU { get; set; }
    public int CategoryId { get; set; }
}