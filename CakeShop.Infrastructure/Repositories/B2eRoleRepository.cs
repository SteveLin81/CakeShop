using CakeShop.Core.Interfaces;
using EC.Entities.Models;
using CakeShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CakeShop.Infrastructure.Repositories;

public class B2eRoleRepository : IB2eRoleRepository
{
    private readonly CakeShopDbContext _ctx;

    public B2eRoleRepository(CakeShopDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<B2eRole>> GetAllAsync()
        => await _ctx.B2eRoles.OrderBy(r => r.Id).AsNoTracking().ToListAsync();

    public async Task<B2eRole?> GetByIdAsync(int id)
        => await _ctx.B2eRoles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

    public async Task<B2eRole> CreateAsync(B2eRole role)
    {
        _ctx.B2eRoles.Add(role);
        await _ctx.SaveChangesAsync();
        return role;
    }

    public async Task<B2eRole> UpdateAsync(B2eRole role)
    {
        _ctx.B2eRoles.Update(role);
        await _ctx.SaveChangesAsync();
        return await _ctx.B2eRoles.AsNoTracking().FirstAsync(r => r.Id == role.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _ctx.B2eRoles.FindAsync(id);
        if (role is not null) { _ctx.B2eRoles.Remove(role); await _ctx.SaveChangesAsync(); }
    }
}
