using EC.Entities.Models;

namespace CakeShop.Core.Interfaces;

public interface IAnnouncementRepository
{
    Task<Announcement?> GetActiveAsync();
    Task<IEnumerable<Announcement>> GetAllAsync();
    Task<Announcement?> GetByIdAsync(int id);
    Task<Announcement>  CreateAsync(Announcement announcement);
    Task<Announcement>  UpdateAsync(Announcement announcement);
    Task                DeleteAsync(int id);
}
