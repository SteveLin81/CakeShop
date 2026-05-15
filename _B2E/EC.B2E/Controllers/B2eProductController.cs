using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.B2E.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EC.B2E.Controllers;

[ApiController]
[Route("api/b2e/products")]
[ServiceFilter(typeof(B2eAuthFilter))]
public class B2eProductController : ControllerBase
{
    private readonly IProductManagementService _mgmtSvc;
    private readonly IProductService           _productSvc;

    public B2eProductController(IProductManagementService mgmtSvc, IProductService productSvc)
    {
        _mgmtSvc    = mgmtSvc;
        _productSvc = productSvc;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var dtos = await _productSvc.GetAllProductsAsync();
        return Ok(ApiResult<IEnumerable<ProductDto>>.Ok(dtos));
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var cats = await _mgmtSvc.GetCategoriesAsync();
        return Ok(ApiResult<IEnumerable<CategoryDto>>.Ok(cats));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _productSvc.GetProductByIdAsync(id);
        if (dto is null) return NotFound(ApiResult<object>.Fail($"商品 {id} 不存在"));
        return Ok(ApiResult<ProductDto>.Ok(dto));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductSaveRequest request)
    {
        var operatorName = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _mgmtSvc.CreateProductAsync(request, operatorName);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id },
                ApiResult<ProductDto>.Ok(dto, "商品新增成功"));
        }
        catch (Exception ex) { return BadRequest(ApiResult<object>.Fail(ex.Message)); }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductSaveRequest request)
    {
        var operatorName = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _mgmtSvc.UpdateProductAsync(id, request, operatorName);
            return Ok(ApiResult<ProductDto>.Ok(dto, "商品更新成功"));
        }
        catch (KeyNotFoundException ex) { return NotFound(ApiResult<object>.Fail(ex.Message)); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mgmtSvc.DeleteProductAsync(id);
        return Ok(ApiResult<object>.Ok(new { }, "商品刪除成功"));
    }
}
