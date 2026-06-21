using System.ComponentModel.DataAnnotations;
using TestProjectMTech.api.Constants;
using TestProjectMTech.api.Domain;

namespace TestProjectMTech.api.DTO.Requests;

public class GetProductsFilters
{
    public int? categoryId { get; set; } = null;
    public Status? status { get; set; } = null;
    
    [Range(PaginationConstants.MinPage, PaginationConstants.MaxPage)]
    public int page { get; set; } = PaginationConstants.DefaultPage;
    
    [Range(PaginationConstants.MinPageSize, PaginationConstants.MaxPageSize)]
    public int pageSize { get; set; } = PaginationConstants.DefaultPageSize;
}
