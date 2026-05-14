using EC.Entities.Models;

namespace CakeShop.Core.Interfaces;

public interface IAnnouncementRepository
{
    Task<Announcement?> GetActiveAsync();
}
