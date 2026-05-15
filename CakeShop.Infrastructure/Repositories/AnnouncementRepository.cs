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

    public async Task<IEnumerable<Announcement>> GetAllAsync()
        => await _ctx.Announcements.AsNoTracking()
               .OrderByDescending(a => a.CreatedAt).ToListAsync();

    public async Task<Announcement?> GetByIdAsync(int id)
        => await _ctx.Announcements.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Announcement> CreateAsync(Announcement announcement)
    {
        _ctx.Announcements.Add(announcement);
        await _ctx.SaveChangesAsync();
        return announcement;
    }

    public async Task<Announcement> UpdateAsync(Announcement announcement)
    {
        _ctx.Announcements.Update(announcement);
        await _ctx.SaveChangesAsync();
        return announcement;
    }

    public async Task DeleteAsync(int id)
    {
        var ann = await _ctx.Announcements.FindAsync(id);
        if (ann is not null)
        {
            _ctx.Announcements.Remove(ann);
            await _ctx.SaveChangesAsync();
        }
    }
}
