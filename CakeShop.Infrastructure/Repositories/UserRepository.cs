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

    public async Task<User?> GetByEmailAsync(string email)
        => await _ctx.Users.AsNoTracking()
               .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

    public async Task<User?> GetByResetTokenAsync(string token)
        => await _ctx.Users.AsNoTracking()
               .FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpires > DateTime.UtcNow);

    public async Task<IEnumerable<User>> GetAllAsync()
        => await _ctx.Users.AsNoTracking()
               .OrderByDescending(u => u.CreatedAt).ToListAsync();

    public async Task<User> CreateAsync(User user)
    {
        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _ctx.Users.Update(user);
        await _ctx.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _ctx.Users.FindAsync(id);
        if (user is not null)
        {
            _ctx.Users.Remove(user);
            await _ctx.SaveChangesAsync();
        }
    }
}
