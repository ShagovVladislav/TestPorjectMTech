using System.ComponentModel.DataAnnotations;
using TestProjectMTech.Api.Constants;
using TestProjectMTech.Api.Domain;

namespace TestProjectMTech.Api.DTO.Requests;

public class GetProductsFilters
{
    public int? CategoryId { get; set; } = null;
    public Status? Status { get; set; } = null;
    
    [Range(PaginationConstants.MinPage, PaginationConstants.MaxPage)]
    public int Page { get; set; } = PaginationConstants.DefaultPage;
    
    [Range(PaginationConstants.MinPageSize, PaginationConstants.MaxPageSize)]
    public int PageSize { get; set; } = PaginationConstants.DefaultPageSize;
}
