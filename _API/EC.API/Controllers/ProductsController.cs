using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EC.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
        => _productService = productService;

    /// <summary>取得所有商品</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _productService.GetAllProductsAsync());

    /// <summary>取得所有分類</summary>
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
        => Ok(await _productService.GetCategoriesAsync());

    /// <summary>依商品 ID 查詢</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product is null ? NotFound(new { message = $"商品 ID {id} 不存在" }) : Ok(product);
    }

    /// <summary>依分類 ID 查詢商品</summary>
    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
        => Ok(await _productService.GetProductsByCategoryAsync(categoryId));
}
