using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface IB2cUserManagementService
{
    Task<IEnumerable<B2cUserDto>> GetAllAsync();
    Task<B2cUserDto?>             GetByIdAsync(int id);
    Task<B2cUserDto>              CreateAsync(B2cUserCreateRequest request, string operatorName);
    Task<B2cUserDto>              UpdateAsync(int id, B2cUserUpdateRequest request, string operatorName);
    Task                          DeleteAsync(int id);
}
