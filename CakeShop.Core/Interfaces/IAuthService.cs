using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface IAuthService
{
    Task<LoginResponse>             LoginAsync(LoginRequest request);
    Task<bool>                      ValidateTokenAsync(string token);
    Task<string?>                   GetUsernameFromTokenAsync(string token);
    Task<(bool ok, string message)> RegisterAsync(RegisterRequest request);
    Task<bool>                      ForgotPasswordAsync(string email, string resetBaseUrl);
    Task<(bool ok, string message)> ResetPasswordAsync(ResetPasswordRequest request);
}
