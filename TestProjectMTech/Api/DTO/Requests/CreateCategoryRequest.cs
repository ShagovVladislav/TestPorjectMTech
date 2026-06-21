using System.ComponentModel.DataAnnotations;
using TestProjectMTech.Api.Constants;

namespace TestProjectMTech.Api.DTO.Requests;

public class CreateCategoryRequest
{
    [Required]
    [MaxLength(RequestStringConstants.CategoryNameMaxLength)]
    [MinLength(RequestStringConstants.CategoryNameMinLength)]
    public required string Name { get; set; }
}
