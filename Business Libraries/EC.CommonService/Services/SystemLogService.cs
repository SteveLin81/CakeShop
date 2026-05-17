using CakeShop.Core.Interfaces;
using CakeShop.Infrastructure.Data;
using EC.Entities.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EC.CommonService.Services;

public class SystemLogService : ISystemLogService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public SystemLogService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

    public async Task WriteAsync(string project, string functionName, string errorMessage,
                                  string username = "system", Exception? ex = null, string level = "Error")
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<CakeShopDbContext>();
            ctx.SystemLogs.Add(new SystemLog
            {
                LogTime      = DateTime.UtcNow,
                Username     = username,
                Project      = project,
                FunctionName = functionName,
                ErrorMessage = errorMessage,
                ExceptionMsg = ex?.ToString(),
                LogLevel     = level,
            });
            await ctx.SaveChangesAsync();
        }
        catch { /* 靜默，避免無限遞迴 */ }
    }
}
