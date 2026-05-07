using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<bool> ValidateTokenAsync(string token);
    Task<string?> GetUsernameFromTokenAsync(string token);
}
