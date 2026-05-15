using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.B2E.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EC.B2E.Controllers;

[ApiController]
[Route("api/b2e/announcements")]
[ServiceFilter(typeof(B2eAuthFilter))]
public class B2eAnnouncementController : ControllerBase
{
    private readonly IAnnouncementManagementService _mgmtSvc;
    private readonly ILogger<B2eAnnouncementController> _logger;

    public B2eAnnouncementController(IAnnouncementManagementService mgmtSvc,
        ILogger<B2eAnnouncementController> logger)
    {
        _mgmtSvc = mgmtSvc;
        _logger  = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _mgmtSvc.GetAllAsync();
        return Ok(ApiResult<IEnumerable<AnnouncementDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _mgmtSvc.GetByIdAsync(id);
        if (dto is null) return NotFound(ApiResult<object>.Fail($"公告 {id} 不存在"));
        return Ok(ApiResult<AnnouncementDto>.Ok(dto));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AnnouncementSaveRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        var op  = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _mgmtSvc.CreateAsync(request, op);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id },
                ApiResult<AnnouncementDto>.Ok(dto, "公告新增成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "公告新增失敗");
            return BadRequest(ApiResult<object>.Fail("公告新增失敗，請稍後再試"));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AnnouncementSaveRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        var op = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _mgmtSvc.UpdateAsync(id, request, op);
            return Ok(ApiResult<AnnouncementDto>.Ok(dto, "公告更新成功"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResult<object>.Fail($"公告 {id} 不存在"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "公告更新失敗 id={Id}", id);
            return BadRequest(ApiResult<object>.Fail("公告更新失敗，請稍後再試"));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mgmtSvc.DeleteAsync(id);
        return Ok(ApiResult<object>.Ok(new { }, "公告刪除成功"));
    }

    [HttpPatch("{id:int}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        var op = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            await _mgmtSvc.SetActiveAsync(id, op);
            return Ok(ApiResult<object>.Ok(new { }, "已設為啟用公告"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResult<object>.Fail($"公告 {id} 不存在"));
        }
    }
}
