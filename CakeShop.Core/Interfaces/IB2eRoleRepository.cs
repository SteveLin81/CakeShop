using EC.Entities.Models;

namespace CakeShop.Core.Interfaces;

public interface IB2eRoleRepository
{
    Task<IEnumerable<B2eRole>> GetAllAsync();
    Task<B2eRole?>             GetByIdAsync(int id);
    Task<B2eRole>              CreateAsync(B2eRole role);
    Task<B2eRole>              UpdateAsync(B2eRole role);
    Task                       DeleteAsync(int id);
}
