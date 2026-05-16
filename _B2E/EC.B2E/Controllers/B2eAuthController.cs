using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.B2E.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EC.B2E.Controllers;

[ApiController]
[Route("api/b2e/auth")]
public class B2eAuthController : ControllerBase
{
    private readonly IB2eAuthService _authService;

    public B2eAuthController(IB2eAuthService authService) => _authService = authService;

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] ValidateTokenRequest request)
    {
        var isValid = await _authService.ValidateTokenAsync(request.Token);
        return Ok(new { isValid });
    }

    /// <summary>取得目前登入的後台帳號資訊（含角色與權限）</summary>
    [HttpGet("me")]
    [ServiceFilter(typeof(B2eAuthFilter))]
    [ExemptFromPasswordChange]
    public async Task<IActionResult> Me()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
        var me    = await _authService.GetMeAsync(token);
        if (me is null) return Unauthorized();
        return Ok(ApiResult<B2eAdminDto>.Ok(me));
    }

    /// <summary>修改密碼（需 Token；必須改密碼的帳號也可呼叫）</summary>
    [HttpPost("change-password")]
    [ServiceFilter(typeof(B2eAuthFilter))]
    [ExemptFromPasswordChange]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResult<object>.Fail("請求資料格式錯誤"));
        var username = HttpContext.Items["b2e-username"]?.ToString()!;
        var ok = await _authService.ChangePasswordAsync(username, request);
        return ok
            ? Ok(ApiResult<object>.Ok(new { }, "密碼修改成功，請重新登入"))
            : BadRequest(ApiResult<object>.Fail("目前密碼錯誤"));
    }
}

public class ValidateTokenRequest { public string Token { get; set; } = string.Empty; }
