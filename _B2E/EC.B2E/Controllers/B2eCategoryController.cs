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
    private readonly ISystemLogService _log;

    public B2eCategoryController(ICategoryManagementService svc, ILogger<B2eCategoryController> logger, ISystemLogService log)
    {
        _svc    = svc;
        _logger = logger;
        _log    = log;
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
        catch (InvalidOperationException ex) { return Conflict(ApiResult<object>.Fail(ex.Message)); }
        catch (KeyNotFoundException ex)      { return NotFound(ApiResult<object>.Fail(ex.Message)); }
        catch (Exception ex)
        {
            var username = HttpContext.Items["b2e-username"]?.ToString() ?? "unknown";
            await _log.WriteAsync("B2E", nameof(Create), ex.Message, username, ex);
            return StatusCode(500, ApiResult<object>.Fail($"操作失敗：{ex.Message}"));
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
        catch (InvalidOperationException ex) { return Conflict(ApiResult<object>.Fail(ex.Message)); }
        catch (KeyNotFoundException ex)      { return NotFound(ApiResult<object>.Fail(ex.Message)); }
        catch (Exception ex)
        {
            var username = HttpContext.Items["b2e-username"]?.ToString() ?? "unknown";
            await _log.WriteAsync("B2E", nameof(Update), ex.Message, username, ex);
            return StatusCode(500, ApiResult<object>.Fail($"操作失敗：{ex.Message}"));
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
        catch (InvalidOperationException ex) { return Conflict(ApiResult<object>.Fail(ex.Message)); }
        catch (KeyNotFoundException ex)      { return NotFound(ApiResult<object>.Fail(ex.Message)); }
        catch (Exception ex)
        {
            var username = HttpContext.Items["b2e-username"]?.ToString() ?? "unknown";
            await _log.WriteAsync("B2E", nameof(Delete), ex.Message, username, ex);
            return StatusCode(500, ApiResult<object>.Fail($"操作失敗：{ex.Message}"));
        }
    }
}
