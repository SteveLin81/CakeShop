using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface IAnnouncementService
{
    Task<AnnouncementDto?> GetActiveAnnouncementAsync();
}
