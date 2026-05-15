using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.Entities.Models;

namespace EC.CommonService.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product is not null ? MapToDto(product) : null;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await _productRepository.GetByCategoryAsync(categoryId);
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _productRepository.GetCategoriesAsync();
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            NameEn = c.NameEn,
            NameJa = c.NameJa,
            NameZhCn = c.NameZhCn,
            NameTh = c.NameTh ?? string.Empty,
            NameKo = c.NameKo ?? string.Empty,
            NameVi = c.NameVi ?? string.Empty,
            NameMs = c.NameMs ?? string.Empty
        });
    }

    internal static ProductDto MapToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        NameEn = p.NameEn,
        NameJa = p.NameJa,
        NameZhCn = p.NameZhCn,
        NameTh = p.NameTh ?? string.Empty,
        NameKo = p.NameKo ?? string.Empty,
        NameVi = p.NameVi ?? string.Empty,
        NameMs = p.NameMs ?? string.Empty,
        Description = p.Description,
        DescriptionEn = p.DescriptionEn,
        DescriptionJa = p.DescriptionJa,
        DescriptionZhCn = p.DescriptionZhCn,
        DescriptionTh = p.DescriptionTh ?? string.Empty,
        DescriptionKo = p.DescriptionKo ?? string.Empty,
        DescriptionVi = p.DescriptionVi ?? string.Empty,
        DescriptionMs = p.DescriptionMs ?? string.Empty,
        Price = p.Price,
        ImageUrl = p.ImageUrl,
        CategoryId = p.CategoryId,
        CategoryName = p.Category?.Name ?? string.Empty,
        CategoryNameEn = p.Category?.NameEn ?? string.Empty,
        CategoryNameJa = p.Category?.NameJa ?? string.Empty,
        CategoryNameZhCn = p.Category?.NameZhCn ?? string.Empty,
        CategoryNameTh = p.Category?.NameTh ?? string.Empty,
        CategoryNameKo = p.Category?.NameKo ?? string.Empty,
        CategoryNameVi = p.Category?.NameVi ?? string.Empty,
        CategoryNameMs = p.Category?.NameMs ?? string.Empty,
        IsAvailable = p.IsAvailable
    };
}
