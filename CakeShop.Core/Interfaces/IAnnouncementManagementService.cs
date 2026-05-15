using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface IAnnouncementManagementService
{
    Task<IEnumerable<AnnouncementDto>> GetAllAsync();
    Task<AnnouncementDto?>             GetByIdAsync(int id);
    Task<AnnouncementDto>              CreateAsync(AnnouncementSaveRequest request, string operatorName);
    Task<AnnouncementDto>              UpdateAsync(int id, AnnouncementSaveRequest request, string operatorName);
    Task                               DeleteAsync(int id);
    Task                               SetActiveAsync(int id, string operatorName);
}
