using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface IB2eAdminManagementService
{
    Task<IEnumerable<B2eAdminDto>> GetAllAsync();
    Task<B2eAdminDto?>             GetByIdAsync(int id);
    Task<B2eAdminDto>              CreateAsync(B2eAdminCreateRequest req, string operatorName);
    Task<B2eAdminDto>              UpdateAsync(int id, B2eAdminUpdateRequest req, string operatorName);
    Task                           DeleteAsync(int id);
}
