using CakeShop.Core.Models;

namespace CakeShop.Core.Interfaces;

public interface IAnnouncementRepository
{
    Task<Announcement?> GetActiveAsync();
}
