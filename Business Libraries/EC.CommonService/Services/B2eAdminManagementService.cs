using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.Entities.Models;

namespace EC.CommonService.Services;

public class B2eAdminManagementService : IB2eAdminManagementService
{
    private readonly IB2eUserRepository _repo;
    private readonly IEncryptionService  _enc;

    public B2eAdminManagementService(IB2eUserRepository repo, IEncryptionService enc)
    {
        _repo = repo;
        _enc  = enc;
    }

    public async Task<IEnumerable<B2eAdminDto>> GetAllAsync()
        => (await _repo.GetAllAsync()).Select(B2eAuthService.MapToAdminDto);

    public async Task<B2eAdminDto?> GetByIdAsync(int id)
    {
        var u = await _repo.GetByIdAsync(id);
        return u is null ? null : B2eAuthService.MapToAdminDto(u);
    }

    public async Task<B2eAdminDto> CreateAsync(B2eAdminCreateRequest req, string operatorName)
    {
        var all = await _repo.GetAllAsync();
        if (all.Any(u => u.Username.Equals(req.Username, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("帳號已存在");
        if (!string.IsNullOrWhiteSpace(req.Email) &&
            all.Any(u => u.Email.Equals(req.Email, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("此 Email 已被其他帳號使用");

        var user = new B2eUser
        {
            Username           = req.Username,
            Email              = req.Email,
            PasswordHash       = _enc.HashPassword("0000"),
            RoleId             = req.RoleId,
            MustChangePassword = true,
            CreatedBy          = operatorName,
            UpdatedBy          = operatorName,
        };
        return B2eAuthService.MapToAdminDto(await _repo.CreateAsync(user));
    }

    public async Task<B2eAdminDto> UpdateAsync(int id, B2eAdminUpdateRequest req, string operatorName)
    {
        var existing = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"帳號 {id} 不存在");
        if (!string.IsNullOrWhiteSpace(req.Email))
        {
            var all = await _repo.GetAllAsync();
            if (all.Any(u => u.Id != id && u.Email.Equals(req.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("此 Email 已被其他帳號使用");
        }
        existing.Email     = req.Email;
        existing.RoleId    = req.RoleId;
        existing.UpdatedBy = operatorName;
        return B2eAuthService.MapToAdminDto(await _repo.UpdateAsync(existing));
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
}
