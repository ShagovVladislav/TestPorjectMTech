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
    public async Task<ActionResult<List<Product>>> GetAllProducts([FromQuery] GetProductsFilters filters)
    {
        var product = await _productService.GetProducts(filters);
        
        return Ok(product);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product?>> GetCategoryById([FromRoute] int id)
    {
        var product = await _productService.GetProductById(id);

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductRequest productRequest)
    {
        var product = await _productService.CreateProduct(productRequest);
        
        return Ok(product);
    }
    
    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<Product>> ChangeProductStatus([FromRoute] int id, Status status)
    {
        var product = await _productService.ChangeStatus(id, status);
        
        return Ok(product); 
    }
}