using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;

namespace CakeShop.Business.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _repo;

    public AnnouncementService(IAnnouncementRepository repo) => _repo = repo;

    public async Task<AnnouncementDto?> GetActiveAnnouncementAsync()
    {
        var ann = await _repo.GetActiveAsync();
        if (ann is null) return null;

        return new AnnouncementDto
        {
            Content     = ann.Content,
            ContentEn   = ann.ContentEn,
            ContentJa   = ann.ContentJa,
            ContentZhCn = ann.ContentZhCn,
            IsActive    = ann.IsActive
        };
    }
}
