namespace CakeShop.Core.Interfaces;

public interface ISystemLogService
{
    Task WriteAsync(string project, string functionName, string errorMessage,
                    string username = "system", Exception? ex = null, string level = "Error");
}
