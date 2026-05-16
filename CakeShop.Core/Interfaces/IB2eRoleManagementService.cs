using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface IB2eRoleManagementService
{
    Task<IEnumerable<B2eRoleDto>> GetAllAsync();
    Task<B2eRoleDto?>             GetByIdAsync(int id);
    Task<B2eRoleDto>              CreateAsync(B2eRoleSaveRequest req, string operatorName);
    Task<B2eRoleDto>              UpdateAsync(int id, B2eRoleSaveRequest req, string operatorName);
    Task                          DeleteAsync(int id);
}
