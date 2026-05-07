using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using CakeShop.Core.Models;

namespace CakeShop.Business.Services;

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
            NameZhCn = c.NameZhCn
        });
    }

    private static ProductDto MapToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        NameEn = p.NameEn,
        NameJa = p.NameJa,
        NameZhCn = p.NameZhCn,
        Description = p.Description,
        DescriptionEn = p.DescriptionEn,
        DescriptionJa = p.DescriptionJa,
        DescriptionZhCn = p.DescriptionZhCn,
        Price = p.Price,
        ImageUrl = p.ImageUrl,
        CategoryId = p.CategoryId,
        CategoryName = p.Category?.Name ?? string.Empty,
        CategoryNameEn = p.Category?.NameEn ?? string.Empty,
        CategoryNameJa = p.Category?.NameJa ?? string.Empty,
        CategoryNameZhCn = p.Category?.NameZhCn ?? string.Empty,
        IsAvailable = p.IsAvailable
    };
}
