using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;

namespace CakeShop.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IEncryptionService _encryptionService;

    public AuthService(IUserRepository userRepository, IEncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null || !_encryptionService.VerifyPassword(request.Password, user.PasswordHash))
            return new LoginResponse { Success = false, Message = "帳號或密碼錯誤" };

        // Token payload: userId|username|expiresAt (ISO 8601)
        var payload = $"{user.Id}|{user.Username}|{DateTime.UtcNow.AddHours(24):O}";
        var token = _encryptionService.EncryptAesGcm(payload);

        return new LoginResponse
        {
            Success = true,
            Token = token,
            Username = user.Username,
            Message = "登入成功"
        };
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var payload = _encryptionService.DecryptAesGcm(token);
            var parts = payload.Split('|');
            if (parts.Length < 3) return Task.FromResult(false);
            var expiry = DateTime.Parse(parts[2]);
            return Task.FromResult(expiry > DateTime.UtcNow);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public async Task<string?> GetUsernameFromTokenAsync(string token)
    {
        try
        {
            if (!await ValidateTokenAsync(token)) return null;
            var payload = _encryptionService.DecryptAesGcm(token);
            var parts = payload.Split('|');
            return parts.Length >= 2 ? parts[1] : null;
        }
        catch { return null; }
    }
}
