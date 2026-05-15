using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.Entities.Models;

namespace EC.CommonService.Services;

public class ProductManagementService : IProductManagementService
{
    private readonly IProductRepository _repo;

    public ProductManagementService(IProductRepository repo) => _repo = repo;

    public async Task<ProductDto> CreateProductAsync(ProductSaveRequest req, string operatorName)
    {
        var product = MapToEntity(req);
        product.CreatedBy = operatorName;
        product.UpdatedBy = operatorName;
        var created = await _repo.CreateAsync(product);
        // 重新讀取含 Category 的完整資料
        var full = await _repo.GetByIdAsync(created.Id);
        return ProductService.MapToDto(full!);
    }

    public async Task<ProductDto> UpdateProductAsync(int id, ProductSaveRequest req, string operatorName)
    {
        var existing = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"商品 {id} 不存在");

        MapRequestToEntity(req, existing);
        existing.UpdatedBy = operatorName;
        await _repo.UpdateAsync(existing);
        var full = await _repo.GetByIdAsync(id);
        return ProductService.MapToDto(full!);
    }

    public Task DeleteProductAsync(int id) => _repo.DeleteAsync(id);

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var cats = await _repo.GetCategoriesAsync();
        return cats.Select(c => new CategoryDto
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
        });
    }

    private static Product MapToEntity(ProductSaveRequest r) => new()
    {
        Name            = r.Name,
        NameEn          = r.NameEn,
        NameJa          = r.NameJa,
        NameZhCn        = r.NameZhCn,
        NameTh          = r.NameTh,
        NameKo          = r.NameKo,
        NameVi          = r.NameVi,
        NameMs          = r.NameMs,
        Description     = r.Description,
        DescriptionEn   = r.DescriptionEn,
        DescriptionJa   = r.DescriptionJa,
        DescriptionZhCn = r.DescriptionZhCn,
        DescriptionTh   = r.DescriptionTh,
        DescriptionKo   = r.DescriptionKo,
        DescriptionVi   = r.DescriptionVi,
        DescriptionMs   = r.DescriptionMs,
        Price           = r.Price,
        ImageUrl        = r.ImageUrl,
        CategoryId      = r.CategoryId,
        IsAvailable     = r.IsAvailable,
    };

    private static void MapRequestToEntity(ProductSaveRequest r, Product p)
    {
        p.Name = r.Name; p.NameEn = r.NameEn; p.NameJa = r.NameJa; p.NameZhCn = r.NameZhCn;
        p.NameTh = r.NameTh; p.NameKo = r.NameKo; p.NameVi = r.NameVi; p.NameMs = r.NameMs;
        p.Description = r.Description; p.DescriptionEn = r.DescriptionEn;
        p.DescriptionJa = r.DescriptionJa; p.DescriptionZhCn = r.DescriptionZhCn;
        p.DescriptionTh = r.DescriptionTh; p.DescriptionKo = r.DescriptionKo;
        p.DescriptionVi = r.DescriptionVi; p.DescriptionMs = r.DescriptionMs;
        p.Price = r.Price; p.ImageUrl = r.ImageUrl;
        p.CategoryId = r.CategoryId; p.IsAvailable = r.IsAvailable;
    }
}
