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
        => await _ctx.B2eUsers.AsNoTracking()
               .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

    public async Task<B2eUser?> GetByIdAsync(int id)
        => await _ctx.B2eUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
}
