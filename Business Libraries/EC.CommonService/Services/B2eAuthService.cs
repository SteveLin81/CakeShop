using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using System.Text.Json;

namespace EC.CommonService.Services;

public class B2eAuthService : IB2eAuthService
{
    private readonly IB2eUserRepository  _userRepository;
    private readonly IEncryptionService  _encryptionService;

    public B2eAuthService(IB2eUserRepository userRepository, IEncryptionService encryptionService)
    {
        _userRepository    = userRepository;
        _encryptionService = encryptionService;
    }

    public async Task<B2eLoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null || !_encryptionService.VerifyPassword(request.Password, user.PasswordHash))
            return new B2eLoginResponse { Success = false, Message = "帳號或密碼錯誤" };

        var payload = $"{user.Id}|{user.Username}|{DateTime.UtcNow.AddHours(24):O}";
        var token   = _encryptionService.EncryptAesGcm(payload);
        var perms   = ParsePermissions(user.Role?.Permissions);

        return new B2eLoginResponse
        {
            Success            = true,
            Token              = token,
            Username           = user.Username,
            Message            = "登入成功",
            Role               = user.Role?.Name ?? string.Empty,
            Permissions        = perms,
            MustChangePassword = user.MustChangePassword,
        };
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var payload = _encryptionService.DecryptAesGcm(token);
            var parts   = payload.Split('|');
            if (parts.Length < 3) return Task.FromResult(false);
            var expiry = DateTime.Parse(parts[2], null, System.Globalization.DateTimeStyles.RoundtripKind);
            return Task.FromResult(expiry > DateTime.UtcNow);
        }
        catch { return Task.FromResult(false); }
    }

    public async Task<string?> GetUsernameFromTokenAsync(string token)
    {
        try
        {
            if (!await ValidateTokenAsync(token)) return null;
            var payload = _encryptionService.DecryptAesGcm(token);
            var parts   = payload.Split('|');
            return parts.Length >= 2 ? parts[1] : null;
        }
        catch { return null; }
    }

    public async Task<B2eAdminDto?> GetMeAsync(string token)
    {
        var username = await GetUsernameFromTokenAsync(token);
        if (username is null) return null;
        var user = await _userRepository.GetByUsernameAsync(username);
        return user is null ? null : MapToAdminDto(user);
    }

    public async Task<bool> ChangePasswordAsync(string username, ChangePasswordRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user is null) return false;

        if (!_encryptionService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            return false;

        // detach AsNoTracking entity → fetch tracked version
        var tracked = await _userRepository.GetByIdAsync(user.Id);
        if (tracked is null) return false;

        tracked.PasswordHash       = _encryptionService.HashPassword(request.NewPassword);
        tracked.MustChangePassword = false;
        await _userRepository.UpdateAsync(tracked);
        return true;
    }

    internal static B2eAdminDto MapToAdminDto(EC.Entities.Models.B2eUser u) => new()
    {
        Id                 = u.Id,
        Username           = u.Username,
        Email              = u.Email,
        RoleId             = u.RoleId,
        RoleName           = u.Role?.Name ?? string.Empty,
        Permissions        = ParsePermissions(u.Role?.Permissions),
        MustChangePassword = u.MustChangePassword,
        CreatedAt          = u.CreatedAt,
        UpdatedAt          = u.UpdatedAt,
        UpdateCount        = u.UpdateCount,
    };

    internal static string[] ParsePermissions(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return [];
        try { return JsonSerializer.Deserialize<string[]>(json) ?? []; }
        catch { return []; }
    }
}
