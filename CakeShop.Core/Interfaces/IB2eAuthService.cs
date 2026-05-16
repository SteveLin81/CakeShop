using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface IB2eAuthService
{
    Task<B2eLoginResponse> LoginAsync(LoginRequest request);
    Task<bool>             ValidateTokenAsync(string token);
    Task<string?>          GetUsernameFromTokenAsync(string token);
    Task<B2eAdminDto?>     GetMeAsync(string token);
    Task<bool>             ChangePasswordAsync(string username, ChangePasswordRequest request);
}
