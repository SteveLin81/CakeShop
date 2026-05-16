using EC.Entities.Models;

namespace CakeShop.Core.Interfaces;

public interface IB2eUserRepository
{
    Task<B2eUser?>             GetByUsernameAsync(string username);
    Task<B2eUser?>             GetByIdAsync(int id);
    Task<IEnumerable<B2eUser>> GetAllAsync();
    Task<B2eUser>              CreateAsync(B2eUser user);
    Task<B2eUser>              UpdateAsync(B2eUser user);
    Task                       DeleteAsync(int id);
}
