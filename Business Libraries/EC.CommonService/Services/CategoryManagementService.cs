using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.Entities.Models;

namespace EC.CommonService.Services;

public class CategoryManagementService : ICategoryManagementService
{
    private readonly IProductRepository _repo;

    public CategoryManagementService(IProductRepository repo) => _repo = repo;

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var list = await _repo.GetCategoriesAsync();
        return list.Select(MapToDto);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var cat = await _repo.GetCategoryByIdAsync(id);
        return cat is null ? null : MapToDto(cat);
    }

    public async Task<CategoryDto> CreateAsync(CategorySaveRequest req, string operatorName)
    {
        var cat = MapToEntity(req);
        cat.CreatedBy = operatorName;
        cat.UpdatedBy = operatorName;
        var created = await _repo.CreateCategoryAsync(cat);
        return MapToDto(created);
    }

    public async Task<CategoryDto> UpdateAsync(int id, CategorySaveRequest req, string operatorName)
    {
        var existing = await _repo.GetCategoryByIdAsync(id)
            ?? throw new KeyNotFoundException($"分類 {id} 不存在");
        MapRequestToEntity(req, existing);
        existing.UpdatedBy = operatorName;
        var updated = await _repo.UpdateCategoryAsync(existing);
        return MapToDto(updated);
    }

    public Task DeleteAsync(int id) => _repo.DeleteCategoryAsync(id);

    private static CategoryDto MapToDto(Category c) => new()
    {
        Id       = c.Id,
        Name     = c.Name,
        NameEn   = c.NameEn,
        NameJa   = c.NameJa,
        NameZhCn = c.NameZhCn,
        NameTh   = c.NameTh   ?? string.Empty,
        NameKo   = c.NameKo   ?? string.Empty,
        NameVi   = c.NameVi   ?? string.Empty,
        NameMs   = c.NameMs   ?? string.Empty,
    };

    private static Category MapToEntity(CategorySaveRequest r) => new()
    {
        Name     = r.Name,
        NameEn   = r.NameEn,
        NameJa   = r.NameJa,
        NameZhCn = r.NameZhCn,
        NameTh   = r.NameTh,
        NameKo   = r.NameKo,
        NameVi   = r.NameVi,
        NameMs   = r.NameMs,
    };

    private static void MapRequestToEntity(CategorySaveRequest r, Category c)
    {
        c.Name = r.Name; c.NameEn = r.NameEn; c.NameJa = r.NameJa; c.NameZhCn = r.NameZhCn;
        c.NameTh = r.NameTh; c.NameKo = r.NameKo; c.NameVi = r.NameVi; c.NameMs = r.NameMs;
    }
}
