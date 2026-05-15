using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
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
}

public class ValidateTokenRequest { public string Token { get; set; } = string.Empty; }
