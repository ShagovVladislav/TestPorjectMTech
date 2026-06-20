using Microsoft.AspNetCore.Mvc;
using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;
using TestProjectMTech.api.Services.Interfaces;

namespace TestProjectMTech.api.Controllers;

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
    public async Task<ActionResult<List<Product>>> GetAllProducts([FromQuery] GetProductsFilters filters, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProducts(filters, cancellationToken);
        
        return Ok(product);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductById(id, cancellationToken);

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductRequest productRequest, CancellationToken cancellationToken)
    {
        var product = await _productService.CreateProduct(productRequest, cancellationToken);
        
        return Ok(product);
    }
    
    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<Product>> ChangeProductStatus([FromRoute] int id, Status status, CancellationToken cancellationToken)
    {
        var product = await _productService.ChangeStatus(id, status, cancellationToken);
        
        return Ok(product); 
    }
}
