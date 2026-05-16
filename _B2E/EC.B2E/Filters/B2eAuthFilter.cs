using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EC.B2E.Filters;

/// <summary>標記此 Action 免除「必須先改密碼」的攔截</summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ExemptFromPasswordChangeAttribute : Attribute { }

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

        var me = await _authService.GetMeAsync(token);
        if (me is null)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "B2E 認證失敗，使用者不存在" });
            return;
        }

        context.HttpContext.Items["b2e-username"]    = me.Username;
        context.HttpContext.Items["b2e-permissions"] = me.Permissions ?? [];
        context.HttpContext.Items["b2e-role"]        = me.RoleName;

        // 若帳號需要強制改密碼，只有帶有 ExemptFromPasswordChange 屬性的端點可通過
        var isExempt = context.ActionDescriptor.EndpointMetadata
                              .OfType<ExemptFromPasswordChangeAttribute>().Any();
        if (me.MustChangePassword && !isExempt)
        {
            context.Result = new ObjectResult(new
            {
                code    = "must_change_password",
                message = "請先至「修改密碼」頁面更新密碼後才能使用其他功能"
            }) { StatusCode = 403 };
            return;
        }

        await next();
    }
}

/// <summary>Controller 或 Action 層級的權限輔助擴充</summary>
public static class HttpContextPermissionExtensions
{
    public static bool HasPermission(this Microsoft.AspNetCore.Http.HttpContext ctx, string permission)
    {
        var perms = ctx.Items["b2e-permissions"] as string[] ?? [];
        return perms.Contains(permission);
    }
}
