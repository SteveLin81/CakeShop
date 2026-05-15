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

    public B2eUserController(IB2cUserManagementService mgmtSvc) => _mgmtSvc = mgmtSvc;

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
        var op = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _mgmtSvc.CreateAsync(request, op);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id },
                ApiResult<B2cUserDto>.Ok(dto, "帳號新增成功"));
        }
        catch (InvalidOperationException ex) { return Conflict(ApiResult<object>.Fail(ex.Message)); }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] B2cUserUpdateRequest request)
    {
        var op = HttpContext.Items["b2e-username"]?.ToString() ?? "admin";
        try
        {
            var dto = await _mgmtSvc.UpdateAsync(id, request, op);
            return Ok(ApiResult<B2cUserDto>.Ok(dto, "帳號更新成功"));
        }
        catch (KeyNotFoundException ex) { return NotFound(ApiResult<object>.Fail(ex.Message)); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mgmtSvc.DeleteAsync(id);
        return Ok(ApiResult<object>.Ok(new { }, "帳號刪除成功"));
    }
}
