using CakeShop.Core.Interfaces;
using CakeShop.Core.Models;

namespace CakeShop.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users;

    public UserRepository(IEncryptionService encryptionService)
    {
        _users = new List<User>
        {
            new()
            {
                Id = 1,
                Username = "test",
                PasswordHash = encryptionService.HashPassword("test"),
                Email = "test@cakeshop.com"
            }
        };
    }

    public Task<User?> GetByUsernameAsync(string username)
        => Task.FromResult(_users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)));

    public Task<User?> GetByIdAsync(int id)
        => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
}
