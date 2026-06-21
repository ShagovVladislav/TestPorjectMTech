using System.ComponentModel.DataAnnotations;
using TestProjectMTech.api.Constants;

namespace TestProjectMTech.api.DTO.Requests;

public class CreateCategoryRequest
{
    [Required]
    [MaxLength(RequestStringConstants.CategoryNameMaxLength)]
    [MinLength(RequestStringConstants.CategoryNameMinLength)]
    public required string Name { get; set; }
}
