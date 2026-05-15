using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EC.B2E.Filters;

public class B2eAuthFilter : IAsyncActionFilter
{
    private readonly IB2eAuthService _authService;

    public B2eAuthFilter(IB2eAuthService authService) => _authService = authService;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var token = context.HttpContext.Request.Headers["Authorization"]
                           .ToString().Replace("Bearer ", "").Trim();

        if (string.IsNullOrEmpty(token) || !await _authService.ValidateTokenAsync(token))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "B2E 認證失敗，請重新登入" });
            return;
        }

        var username = await _authService.GetUsernameFromTokenAsync(token);
        context.HttpContext.Items["b2e-username"] = username ?? "admin";

        await next();
    }
}
