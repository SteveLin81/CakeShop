using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;

namespace EC.CommonService.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _repo;

    public AnnouncementService(IAnnouncementRepository repo) => _repo = repo;

    public async Task<AnnouncementDto?> GetActiveAnnouncementAsync()
    {
        var ann = await _repo.GetActiveAsync();
        if (ann is null) return null;

        // 新語系欄位若 DB 尚未有值，退回英文內容
        return new AnnouncementDto
        {
            Content     = ann.Content,
            ContentEn   = ann.ContentEn,
            ContentJa   = ann.ContentJa,
            ContentZhCn = ann.ContentZhCn,
            ContentTh   = ann.ContentTh ?? ann.ContentEn,
            ContentKo   = ann.ContentKo ?? ann.ContentEn,
            ContentVi   = ann.ContentVi ?? ann.ContentEn,
            ContentMs   = ann.ContentMs ?? ann.ContentEn,
            IsActive    = ann.IsActive
        };
    }
}
