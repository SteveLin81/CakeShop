using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.Entities.Models;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace EC.CommonService.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository      _userRepo;
    private readonly IEncryptionService   _enc;
    private readonly IEmailService        _email;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepo, IEncryptionService enc,
                       IEmailService email, ILogger<AuthService> logger)
    {
        _userRepo = userRepo;
        _enc      = enc;
        _email    = email;
        _logger   = logger;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepo.GetByUsernameAsync(request.Username);
        if (user is null || !_enc.VerifyPassword(request.Password, user.PasswordHash))
            return new LoginResponse { Success = false, Message = "帳號或密碼錯誤" };

        var payload = $"{user.Id}|{user.Username}|{DateTime.UtcNow.AddHours(24):O}";
        var token   = _enc.EncryptAesGcm(payload);
        return new LoginResponse { Success = true, Token = token, Username = user.Username, Message = "登入成功" };
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var parts = _enc.DecryptAesGcm(token).Split('|');
            if (parts.Length < 3) return Task.FromResult(false);
            return Task.FromResult(DateTime.Parse(parts[2]) > DateTime.UtcNow);
        }
        catch { return Task.FromResult(false); }
    }

    public async Task<string?> GetUsernameFromTokenAsync(string token)
    {
        try
        {
            if (!await ValidateTokenAsync(token)) return null;
            var parts = _enc.DecryptAesGcm(token).Split('|');
            return parts.Length >= 2 ? parts[1] : null;
        }
        catch { return null; }
    }

    public async Task<(bool ok, string message)> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepo.GetByUsernameAsync(request.Username) is not null)
            return (false, "帳號已存在");

        if (await _userRepo.GetByEmailAsync(request.Email) is not null)
            return (false, "此 Email 已被使用");

        await _userRepo.CreateAsync(new User
        {
            Username     = request.Username,
            DisplayName  = request.Username,
            Email        = request.Email,
            PasswordHash = _enc.HashPassword(request.Password),
            CreatedBy    = request.Username,
            UpdatedBy    = request.Username,
        });
        return (true, "帳號建立成功");
    }

    public async Task<bool> ForgotPasswordAsync(string email, string resetBaseUrl)
    {
        var user = await _userRepo.GetByEmailAsync(email);
        if (user is null) return true; // 不透露帳號是否存在

        var token = GenerateToken();
        await _userRepo.UpdateAsync(new User
        {
            Id                = user.Id,
            Username          = user.Username,
            DisplayName       = user.DisplayName,
            PasswordHash      = user.PasswordHash,
            Email             = user.Email,
            ResetToken        = token,
            ResetTokenExpires = DateTime.UtcNow.AddHours(1),
            CreatedAt         = user.CreatedAt,
            CreatedBy         = user.CreatedBy,
            UpdatedBy         = user.Username,
            UpdateCount       = user.UpdateCount,
        });

        var resetUrl = $"{resetBaseUrl}/reset-password?token={token}";
        var body = $"""
            <div style="font-family:sans-serif;max-width:560px;margin:0 auto">
              <h2 style="color:#2c3e50">🎂 CakeShop 密碼重設</h2>
              <p>您好 <b>{System.Net.WebUtility.HtmlEncode(user.Username)}</b>，</p>
              <p>我們收到您的密碼重設請求，請在 <b>1 小時內</b>點擊下方按鈕：</p>
              <p style="text-align:center;margin:24px 0">
                <a href="{resetUrl}" style="background:#27ae60;color:white;padding:12px 28px;border-radius:8px;text-decoration:none;font-weight:600">重設密碼</a>
              </p>
              <p style="color:#7f8c8d;font-size:.85rem">若您未提出此請求，請忽略此郵件，密碼不會更改。</p>
            </div>
            """;
        try
        {
            await _email.SendAsync(email, "【CakeShop】密碼重設連結", body);
            _logger.LogInformation("B2C 忘記密碼發信成功 {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "B2C 忘記密碼發信失敗 {Email}，錯誤：{Msg}", email, ex.Message);
            _logger.LogWarning("【開發用】重設連結：{Url}", resetUrl);
        }
        return true;
    }

    public async Task<(bool ok, string message)> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userRepo.GetByResetTokenAsync(request.Token);
        if (user is null) return (false, "重設連結無效或已過期");

        await _userRepo.UpdateAsync(new User
        {
            Id                = user.Id,
            Username          = user.Username,
            DisplayName       = user.DisplayName,
            PasswordHash      = _enc.HashPassword(request.NewPassword),
            Email             = user.Email,
            ResetToken        = null,
            ResetTokenExpires = null,
            CreatedAt         = user.CreatedAt,
            CreatedBy         = user.CreatedBy,
            UpdatedBy         = user.Username,
            UpdateCount       = user.UpdateCount,
        });
        return (true, "密碼重設成功");
    }

    private static string GenerateToken()
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes).ToLower();
    }
}
