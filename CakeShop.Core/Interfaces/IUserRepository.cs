using EC.Entities.Models;

namespace CakeShop.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByResetTokenAsync(string token);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User>  CreateAsync(User user);
    Task<User>  UpdateAsync(User user);
    Task        DeleteAsync(int id);
}
