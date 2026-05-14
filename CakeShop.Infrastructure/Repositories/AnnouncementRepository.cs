using CakeShop.Core.Interfaces;
using EC.Entities.Models;
using CakeShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CakeShop.Infrastructure.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly CakeShopDbContext _ctx;

    public AnnouncementRepository(CakeShopDbContext ctx) => _ctx = ctx;

    public async Task<Announcement?> GetActiveAsync()
        => await _ctx.Announcements.AsNoTracking()
               .FirstOrDefaultAsync(a => a.IsActive);
}
