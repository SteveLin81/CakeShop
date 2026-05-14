using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EC.B2C.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>取得所有產品</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        => Ok(await _productService.GetAllProductsAsync());

    /// <summary>取得單一產品</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product is not null ? Ok(product) : NotFound(new { message = "找不到此產品" });
    }

    /// <summary>取得所有分類</summary>
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        => Ok(await _productService.GetCategoriesAsync());

    /// <summary>依分類取得產品</summary>
    [HttpGet("category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetByCategory(int categoryId)
        => Ok(await _productService.GetProductsByCategoryAsync(categoryId));
}
