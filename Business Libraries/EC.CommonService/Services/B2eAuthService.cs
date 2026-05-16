using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text.Json;

namespace EC.CommonService.Services;

public class B2eAuthService : IB2eAuthService
{
    private readonly IB2eUserRepository       _userRepository;
    private readonly IEncryptionService       _encryptionService;
    private readonly IEmailService            _email;
    private readonly ILogger<B2eAuthService>  _logger;

    public B2eAuthService(IB2eUserRepository userRepository, IEncryptionService encryptionService,
                          IEmailService email, ILogger<B2eAuthService> logger)
    {
        _userRepository    = userRepository;
        _encryptionService = encryptionService;
        _email             = email;
        _logger            = logger;
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

    public async Task<bool> ForgotPasswordAsync(string email, string resetBaseUrl)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user is null) return true; // 不透露帳號是否存在

        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        var token = Convert.ToHexString(bytes).ToLower();

        user.ResetToken        = token;
        user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);
        await _userRepository.UpdateAsync(user);

        var resetUrl = $"{resetBaseUrl}/b2e/reset-password?token={token}";
        var body = $"""
            <div style="font-family:sans-serif;max-width:560px;margin:0 auto">
              <h2 style="color:#1a252f">⚙️ B2E 後台 密碼重設</h2>
              <p>您好 <b>{System.Net.WebUtility.HtmlEncode(user.Username)}</b>，</p>
              <p>我們收到您的後台密碼重設請求，請在 <b>1 小時內</b>點擊下方按鈕：</p>
              <p style="text-align:center;margin:24px 0">
                <a href="{resetUrl}" style="background:#3498db;color:white;padding:12px 28px;border-radius:8px;text-decoration:none;font-weight:600">重設後台密碼</a>
              </p>
              <p style="color:#7f8c8d;font-size:.85rem">若您未提出此請求，請忽略此郵件。</p>
            </div>
            """;
        try { await _email.SendAsync(email, "【CakeShop B2E】後台密碼重設連結", body); }
        catch (Exception ex) { _logger.LogWarning(ex, "B2E 忘記密碼發信失敗 {Email}", email); }
        return true;
    }

    public async Task<(bool ok, string message)> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userRepository.GetByResetTokenAsync(request.Token);
        if (user is null) return (false, "重設連結無效或已過期");

        user.PasswordHash       = _encryptionService.HashPassword(request.NewPassword);
        user.MustChangePassword = false;
        user.ResetToken         = null;
        user.ResetTokenExpires  = null;
        await _userRepository.UpdateAsync(user);
        return (true, "密碼重設成功");
    }

    internal static string[] ParsePermissions(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return [];
        try { return JsonSerializer.Deserialize<string[]>(json) ?? []; }
        catch { return []; }
    }
}
