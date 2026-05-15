using EC.Entities.Models;

namespace CakeShop.Core.Interfaces;

public interface IB2eUserRepository
{
    Task<B2eUser?> GetByUsernameAsync(string username);
    Task<B2eUser?> GetByIdAsync(int id);
}
