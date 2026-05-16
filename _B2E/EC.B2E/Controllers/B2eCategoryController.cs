using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.B2E.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EC.B2E.Controllers;

[ApiController]
[Route("api/b2e/categories")]
[ServiceFilter(typeof(B2eAuthFilter))]
public class B2eCategoryController : ControllerBase
{
    private readonly ICategoryManagementService _svc;
    private readonly ILogger<B2eCategoryController> _logger;

    public B2eCategoryController(ICategoryManagementService svc, ILogger<B2eCategoryController> logger)
    {
        _svc    = svc;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _svc.GetAllAsync();
        return Ok(ApiResult<IEnumerable<CategoryDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _svc.GetByIdAsync(id);
        if (dto is null) return NotFound(ApiResult<object>.Fail($"分類 {id} 不存在"));
        return Ok(ApiResult<CategoryDto>.Ok(dto));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategorySaveRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        var op = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _svc.CreateAsync(request, op);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id },
                ApiResult<CategoryDto>.Ok(dto, "分類新增成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "分類新增失敗");
            return BadRequest(ApiResult<object>.Fail("分類新增失敗，請稍後再試"));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategorySaveRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        var op = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _svc.UpdateAsync(id, request, op);
            return Ok(ApiResult<CategoryDto>.Ok(dto, "分類更新成功"));
        }
        catch (KeyNotFoundException) { return NotFound(ApiResult<object>.Fail($"分類 {id} 不存在")); }
        catch (Exception ex)
        {
            _logger.LogError(ex, "分類更新失敗 id={Id}", id);
            return BadRequest(ApiResult<object>.Fail("分類更新失敗，請稍後再試"));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _svc.DeleteAsync(id);
            return Ok(ApiResult<object>.Ok(new { }, "分類刪除成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "分類刪除失敗 id={Id}", id);
            return BadRequest(ApiResult<object>.Fail("分類刪除失敗（可能有商品使用此分類）"));
        }
    }
}
