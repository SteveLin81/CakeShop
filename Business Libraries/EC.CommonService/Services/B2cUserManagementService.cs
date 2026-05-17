using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.Entities.Models;

namespace EC.CommonService.Services;

public class B2cUserManagementService : IB2cUserManagementService
{
    private readonly IUserRepository    _userRepo;
    private readonly IEncryptionService _encryption;

    public B2cUserManagementService(IUserRepository userRepo, IEncryptionService encryption)
    {
        _userRepo   = userRepo;
        _encryption = encryption;
    }

    public async Task<IEnumerable<B2cUserDto>> GetAllAsync()
    {
        var users = await _userRepo.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<B2cUserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        return user is null ? null : MapToDto(user);
    }

    public async Task<B2cUserDto> CreateAsync(B2cUserCreateRequest req, string operatorName)
    {
        var existing = await _userRepo.GetByUsernameAsync(req.Username);
        if (existing is not null)
            throw new InvalidOperationException("帳號已存在");
        if (!string.IsNullOrWhiteSpace(req.Email))
        {
            var all = await _userRepo.GetAllAsync();
            if (all.Any(u => !string.IsNullOrWhiteSpace(u.Email) &&
                             u.Email.Equals(req.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("此 Email 已被其他帳號使用");
        }

        var user = new User
        {
            Username     = req.Username,
            PasswordHash = _encryption.HashPassword(req.Password),
            Email        = req.Email,
            CreatedBy    = operatorName,
            UpdatedBy    = operatorName,
        };
        var created = await _userRepo.CreateAsync(user);
        return MapToDto(created);
    }

    public async Task<B2cUserDto> UpdateAsync(int id, B2cUserUpdateRequest req, string operatorName)
    {
        var user = await _userRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"會員 {id} 不存在");

        if (!string.IsNullOrWhiteSpace(req.Email))
        {
            var all = await _userRepo.GetAllAsync();
            if (all.Any(u => u.Id != id && !string.IsNullOrWhiteSpace(u.Email) &&
                             u.Email.Equals(req.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("此 Email 已被其他帳號使用");
        }

        user.Email     = req.Email;
        user.UpdatedBy = operatorName;
        if (!string.IsNullOrWhiteSpace(req.NewPassword))
            user.PasswordHash = _encryption.HashPassword(req.NewPassword);

        var updated = await _userRepo.UpdateAsync(user);
        return MapToDto(updated);
    }

    public Task DeleteAsync(int id) => _userRepo.DeleteAsync(id);

    private static B2cUserDto MapToDto(User u) => new()
    {
        Id          = u.Id,
        Username    = u.Username,
        Email       = u.Email,
        CreatedAt   = u.CreatedAt,
        UpdatedAt   = u.UpdatedAt,
        UpdateCount = u.UpdateCount,
    };
}
