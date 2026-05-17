using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.Entities.Models;

namespace EC.CommonService.Services;

public class AnnouncementManagementService : IAnnouncementManagementService
{
    private readonly IAnnouncementRepository _repo;

    public AnnouncementManagementService(IAnnouncementRepository repo) => _repo = repo;

    public async Task<IEnumerable<AnnouncementDto>> GetAllAsync()
    {
        var list = await _repo.GetAllAsync();
        return list.Select(MapToDto);
    }

    public async Task<AnnouncementDto?> GetByIdAsync(int id)
    {
        var ann = await _repo.GetByIdAsync(id);
        return ann is null ? null : MapToDto(ann);
    }

    public async Task<AnnouncementDto> CreateAsync(AnnouncementSaveRequest req, string operatorName)
    {
        var ann = MapToEntity(req);
        ann.CreatedBy = operatorName;
        ann.UpdatedBy = operatorName;
        var created = await _repo.CreateAsync(ann);
        return MapToDto(created);
    }

    public async Task<AnnouncementDto> UpdateAsync(int id, AnnouncementSaveRequest req, string operatorName)
    {
        var existing = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"公告 {id} 不存在");

        MapRequestToEntity(req, existing);
        existing.UpdatedBy = operatorName;
        var updated = await _repo.UpdateAsync(existing);
        return MapToDto(updated);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

    public async Task SetActiveAsync(int id, string operatorName)
    {
        var all = await _repo.GetAllAsync();
        foreach (var ann in all)
        {
            var newActive = ann.Id == id;
            if (ann.IsActive != newActive)
            {
                ann.IsActive  = newActive;
                ann.UpdatedBy = operatorName;
                await _repo.UpdateAsync(ann);
            }
        }
        // 若目標公告不在列表中（例如 id 不存在），確保啟用
        if (!all.Any(a => a.Id == id))
            throw new KeyNotFoundException($"公告 {id} 不存在");
    }

    private static AnnouncementDto MapToDto(Announcement a) => new()
    {
        Id          = a.Id,
        Content     = a.Content,
        ContentEn   = a.ContentEn,
        ContentJa   = a.ContentJa,
        ContentZhCn = a.ContentZhCn,
        ContentTh   = a.ContentTh ?? a.ContentEn,
        ContentKo   = a.ContentKo ?? a.ContentEn,
        ContentVi   = a.ContentVi ?? a.ContentEn,
        ContentMs   = a.ContentMs ?? a.ContentEn,
        IsActive    = a.IsActive,
    };

    private static Announcement MapToEntity(AnnouncementSaveRequest r) => new()
    {
        Content     = r.Content,
        ContentEn   = r.ContentEn,
        ContentJa   = r.ContentJa,
        ContentZhCn = r.ContentZhCn,
        ContentTh   = r.ContentTh,
        ContentKo   = r.ContentKo,
        ContentVi   = r.ContentVi,
        ContentMs   = r.ContentMs,
        IsActive    = r.IsActive,
    };

    private static void MapRequestToEntity(AnnouncementSaveRequest r, Announcement a)
    {
        a.Content = r.Content; a.ContentEn = r.ContentEn; a.ContentJa = r.ContentJa;
        a.ContentZhCn = r.ContentZhCn; a.ContentTh = r.ContentTh; a.ContentKo = r.ContentKo;
        a.ContentVi = r.ContentVi; a.ContentMs = r.ContentMs; a.IsActive = r.IsActive;
    }
}
