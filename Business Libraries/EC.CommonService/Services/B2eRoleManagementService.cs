using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.Entities.Models;
using System.Text.Json;

namespace EC.CommonService.Services;

public class B2eRoleManagementService : IB2eRoleManagementService
{
    private readonly IB2eRoleRepository _repo;

    public B2eRoleManagementService(IB2eRoleRepository repo) => _repo = repo;

    public async Task<IEnumerable<B2eRoleDto>> GetAllAsync()
        => (await _repo.GetAllAsync()).Select(MapToDto);

    public async Task<B2eRoleDto?> GetByIdAsync(int id)
    {
        var r = await _repo.GetByIdAsync(id);
        return r is null ? null : MapToDto(r);
    }

    public async Task<B2eRoleDto> CreateAsync(B2eRoleSaveRequest req, string operatorName)
    {
        var role = new B2eRole
        {
            Name        = req.Name,
            Description = req.Description,
            Permissions = JsonSerializer.Serialize(req.Permissions),
            CreatedBy   = operatorName,
            UpdatedBy   = operatorName,
        };
        return MapToDto(await _repo.CreateAsync(role));
    }

    public async Task<B2eRoleDto> UpdateAsync(int id, B2eRoleSaveRequest req, string operatorName)
    {
        var existing = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"角色 {id} 不存在");
        existing.Name        = req.Name;
        existing.Description = req.Description;
        existing.Permissions = JsonSerializer.Serialize(req.Permissions);
        existing.UpdatedBy   = operatorName;
        return MapToDto(await _repo.UpdateAsync(existing));
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

    private static B2eRoleDto MapToDto(B2eRole r) => new()
    {
        Id          = r.Id,
        Name        = r.Name,
        Description = r.Description,
        Permissions = B2eAuthService.ParsePermissions(r.Permissions),
    };
}
