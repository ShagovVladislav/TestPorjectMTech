using Microsoft.AspNetCore.Mvc;
using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.DTO.Responses;
using TestProjectMTech.Api.DTO.Responses.Mappers;
using TestProjectMTech.Api.Services.Interfaces;

namespace TestProjectMTech.Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductControllers : ControllerBase
{
    private readonly IProductService _productService;

    public ProductControllers(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAllProducts([FromQuery] GetProductsFilters filters, CancellationToken cancellationToken)
    {
        var products = await _productService.GetProducts(filters, cancellationToken);
        
        return Ok(products.ToResponse());
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetProductById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductById(id, cancellationToken);

        return Ok(product.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> CreateProduct(CreateProductRequest productRequest, CancellationToken cancellationToken)
    {
        var product = await _productService.CreateProduct(productRequest, cancellationToken);
        
        return CreatedAtAction(
            nameof(GetProductById),
            new { id = product.Id },
            product.ToResponse());
    }
    
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<ProductResponse>> ChangeProductStatus([FromRoute] int id, [FromQuery] Status status, CancellationToken cancellationToken)
    {
        var product = await _productService.ChangeStatus(id, status, cancellationToken);
        
        return Ok(product.ToResponse()); 
    }
}
