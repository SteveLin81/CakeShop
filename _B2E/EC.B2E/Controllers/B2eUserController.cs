using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.B2E.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EC.B2E.Controllers;

[ApiController]
[Route("api/b2e/users")]
[ServiceFilter(typeof(B2eAuthFilter))]
public class B2eUserController : ControllerBase
{
    private readonly IB2cUserManagementService _mgmtSvc;
    private readonly ILogger<B2eUserController> _logger;
    private readonly ISystemLogService _log;

    public B2eUserController(IB2cUserManagementService mgmtSvc, ILogger<B2eUserController> logger, ISystemLogService log)
    {
        _mgmtSvc = mgmtSvc;
        _logger  = logger;
        _log     = log;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _mgmtSvc.GetAllAsync();
        return Ok(ApiResult<IEnumerable<B2cUserDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _mgmtSvc.GetByIdAsync(id);
        if (dto is null) return NotFound(ApiResult<object>.Fail($"會員 {id} 不存在"));
        return Ok(ApiResult<B2cUserDto>.Ok(dto));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] B2cUserCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        var op = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _mgmtSvc.CreateAsync(request, op);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id },
                ApiResult<B2cUserDto>.Ok(dto, "帳號新增成功"));
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
    public async Task<IActionResult> Update(int id, [FromBody] B2cUserUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        var op = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _mgmtSvc.UpdateAsync(id, request, op);
            return Ok(ApiResult<B2cUserDto>.Ok(dto, "帳號更新成功"));
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
            await _mgmtSvc.DeleteAsync(id);
            return Ok(ApiResult<object>.Ok(new { }, "帳號刪除成功"));
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
