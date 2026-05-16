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
    private readonly ILogger<B2eProductController> _logger;

    public B2eProductController(IProductManagementService mgmtSvc, IProductService productSvc,
        ILogger<B2eProductController> logger)
    {
        _mgmtSvc    = mgmtSvc;
        _productSvc = productSvc;
        _logger     = logger;
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
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        var operatorName = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _mgmtSvc.CreateProductAsync(request, operatorName);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id },
                ApiResult<ProductDto>.Ok(dto, "商品新增成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "商品新增失敗");
            return BadRequest(ApiResult<object>.Fail("商品新增失敗，請稍後再試"));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductSaveRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        var operatorName = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _mgmtSvc.UpdateProductAsync(id, request, operatorName);
            return Ok(ApiResult<ProductDto>.Ok(dto, "商品更新成功"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResult<object>.Fail($"商品 {id} 不存在"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "商品更新失敗 id={Id}", id);
            return BadRequest(ApiResult<object>.Fail("商品更新失敗，請稍後再試"));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _mgmtSvc.DeleteProductAsync(id);
            return Ok(ApiResult<object>.Ok(new { }, "商品刪除成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "商品刪除失敗 id={Id}", id);
            return BadRequest(ApiResult<object>.Fail("商品刪除失敗，請稍後再試"));
        }
    }

    [HttpPost("upload-image")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(ApiResult<object>.Fail("請選擇圖片檔案"));

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (ext is not (".jpg" or ".jpeg" or ".png" or ".webp" or ".gif"))
            return BadRequest(ApiResult<object>.Fail("僅支援 jpg / png / webp / gif 格式"));

        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
        Directory.CreateDirectory(uploadDir);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadDir, fileName);

        await using var stream = System.IO.File.Create(filePath);
        await file.CopyToAsync(stream);

        var url = $"/uploads/products/{fileName}";
        return Ok(ApiResult<object>.Ok(new { url }, "圖片上傳成功"));
    }
}
