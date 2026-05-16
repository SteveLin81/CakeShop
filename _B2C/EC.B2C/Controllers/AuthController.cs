using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EC.B2C.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService  _authService;
    private readonly IConfiguration _config;

    public AuthController(IAuthService authService, IConfiguration config)
    {
        _authService = authService;
        _config      = config;
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("validate")]
    public async Task<ActionResult<object>> Validate([FromBody] ValidateTokenRequest request)
    {
        var isValid  = await _authService.ValidateTokenAsync(request.Token);
        var username = isValid ? await _authService.GetUsernameFromTokenAsync(request.Token) : null;
        return Ok(new { isValid, username });
    }

    [HttpPost("register")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "請確認輸入格式" });
        var (ok, msg) = await _authService.RegisterAsync(request);
        return ok ? Ok(new { success = true, message = msg })
                  : Conflict(new { success = false, message = msg });
    }

    [HttpPost("forgot-password")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "Email 格式不正確" });
        var baseUrl = _config["App:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        await _authService.ForgotPasswordAsync(request.Email, baseUrl);
        return Ok(new { success = true, message = "若此 Email 已註冊，重設連結已寄出，請查看信箱" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "請確認輸入格式" });
        var (ok, msg) = await _authService.ResetPasswordAsync(request);
        return ok ? Ok(new { success = true, message = msg })
                  : BadRequest(new { success = false, message = msg });
    }
}

public class ValidateTokenRequest
{
    public string Token { get; set; } = string.Empty;
}
