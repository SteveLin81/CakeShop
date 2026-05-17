using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.B2E.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EC.B2E.Controllers;

[ApiController]
[Route("api/b2e/admins")]
[ServiceFilter(typeof(B2eAuthFilter))]
public class B2eAdminController : ControllerBase
{
    private readonly IB2eAdminManagementService _svc;
    private readonly ILogger<B2eAdminController> _logger;
    private readonly ISystemLogService _log;

    public B2eAdminController(IB2eAdminManagementService svc, ILogger<B2eAdminController> logger, ISystemLogService log)
    {
        _svc    = svc;
        _logger = logger;
        _log    = log;
    }

    private IActionResult NoPermission() =>
        StatusCode(403, ApiResult<object>.Fail("沒有後台帳號管理權限"));

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!HttpContext.HasPermission(B2ePermissions.Admins)) return NoPermission();
        var list = await _svc.GetAllAsync();
        return Ok(ApiResult<IEnumerable<B2eAdminDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!HttpContext.HasPermission(B2ePermissions.Admins)) return NoPermission();
        var dto = await _svc.GetByIdAsync(id);
        if (dto is null) return NotFound(ApiResult<object>.Fail($"帳號 {id} 不存在"));
        return Ok(ApiResult<B2eAdminDto>.Ok(dto));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] B2eAdminCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        if (!HttpContext.HasPermission(B2ePermissions.Admins)) return NoPermission();
        var op = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _svc.CreateAsync(request, op);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id },
                ApiResult<B2eAdminDto>.Ok(dto, "後台帳號新增成功（預設密碼：0000）"));
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
    public async Task<IActionResult> Update(int id, [FromBody] B2eAdminUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        if (!HttpContext.HasPermission(B2ePermissions.Admins)) return NoPermission();
        var op = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _svc.UpdateAsync(id, request, op);
            return Ok(ApiResult<B2eAdminDto>.Ok(dto, "後台帳號更新成功"));
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
        if (!HttpContext.HasPermission(B2ePermissions.Admins)) return NoPermission();
        try
        {
            await _svc.DeleteAsync(id);
            return Ok(ApiResult<object>.Ok(new { }, "後台帳號刪除成功"));
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
