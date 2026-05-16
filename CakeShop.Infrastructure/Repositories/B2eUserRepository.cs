using CakeShop.Core.Interfaces;
using EC.Entities.Models;
using CakeShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CakeShop.Infrastructure.Repositories;

public class B2eUserRepository : IB2eUserRepository
{
    private readonly CakeShopDbContext _ctx;

    public B2eUserRepository(CakeShopDbContext ctx) => _ctx = ctx;

    public async Task<B2eUser?> GetByUsernameAsync(string username)
        => await _ctx.B2eUsers.Include(u => u.Role).AsNoTracking()
               .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

    public async Task<B2eUser?> GetByIdAsync(int id)
        => await _ctx.B2eUsers.Include(u => u.Role).AsNoTracking()
               .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<IEnumerable<B2eUser>> GetAllAsync()
        => await _ctx.B2eUsers.Include(u => u.Role)
               .OrderBy(u => u.Username).AsNoTracking().ToListAsync();

    public async Task<B2eUser> CreateAsync(B2eUser user)
    {
        _ctx.B2eUsers.Add(user);
        await _ctx.SaveChangesAsync();
        return (await GetByIdAsync(user.Id))!;
    }

    public async Task<B2eUser> UpdateAsync(B2eUser user)
    {
        _ctx.B2eUsers.Update(user);
        await _ctx.SaveChangesAsync();
        return (await GetByIdAsync(user.Id))!;
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _ctx.B2eUsers.FindAsync(id);
        if (user is not null) { _ctx.B2eUsers.Remove(user); await _ctx.SaveChangesAsync(); }
    }
}
