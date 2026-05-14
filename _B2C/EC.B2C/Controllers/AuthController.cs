using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EC.B2C.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>使用者登入（密碼以 SHA-256 雜湊，Token 以 AES-256-GCM 加密）</summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    /// <summary>驗證 Token 是否有效</summary>
    [HttpPost("validate")]
    public async Task<ActionResult<object>> Validate([FromBody] ValidateTokenRequest request)
    {
        var isValid = await _authService.ValidateTokenAsync(request.Token);
        var username = isValid ? await _authService.GetUsernameFromTokenAsync(request.Token) : null;
        return Ok(new { isValid, username });
    }
}

public class ValidateTokenRequest
{
    public string Token { get; set; } = string.Empty;
}
