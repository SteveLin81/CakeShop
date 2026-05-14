using CakeShop.Core.Interfaces;
using EC.Entities.Models;
using CakeShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CakeShop.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CakeShopDbContext _ctx;

    public UserRepository(CakeShopDbContext ctx) => _ctx = ctx;

    public async Task<User?> GetByUsernameAsync(string username)
        => await _ctx.Users.AsNoTracking()
               .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

    public async Task<User?> GetByIdAsync(int id)
        => await _ctx.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
}
