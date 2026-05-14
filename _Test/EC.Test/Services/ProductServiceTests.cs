using CakeShop.Core.Interfaces;
using EC.CommonService.Services;
using EC.Entities.Models;
using FluentAssertions;
using Moq;

namespace EC.Test.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly ProductService           _sut;

    public ProductServiceTests()
        => _sut = new ProductService(_repoMock.Object);

    // ── GetAllProductsAsync ──────────────────────────────────────────

    [Fact]
    public async Task GetAllProductsAsync_ReturnsAllProductsAsDtos()
    {
        _repoMock.Setup(r => r.GetAllAsync())
                 .ReturnsAsync(BuildProducts(3));

        var result = await _sut.GetAllProductsAsync();

        result.Should().HaveCount(3);
        result.Select(p => p.Id).Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task GetAllProductsAsync_EmptyRepository_ReturnsEmpty()
    {
        _repoMock.Setup(r => r.GetAllAsync())
                 .ReturnsAsync(Enumerable.Empty<Product>());

        var result = await _sut.GetAllProductsAsync();

        result.Should().BeEmpty();
    }

    // ── GetProductByIdAsync ──────────────────────────────────────────

    [Fact]
    public async Task GetProductByIdAsync_ExistingId_ReturnsDto()
    {
        var product = BuildProduct(5);
        _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(product);

        var result = await _sut.GetProductByIdAsync(5);

        result.Should().NotBeNull();
        result!.Id.Should().Be(5);
        result.Name.Should().Be("商品5");
        result.Price.Should().Be(100m * 5);
    }

    [Fact]
    public async Task GetProductByIdAsync_NonExistingId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

        var result = await _sut.GetProductByIdAsync(999);

        result.Should().BeNull();
    }

    // ── GetProductsByCategoryAsync ────────────────────────────────────

    [Fact]
    public async Task GetProductsByCategoryAsync_ReturnsFilteredProducts()
    {
        var products = BuildProducts(4, categoryId: 2);
        _repoMock.Setup(r => r.GetByCategoryAsync(2)).ReturnsAsync(products);

        var result = await _sut.GetProductsByCategoryAsync(2);

        result.Should().HaveCount(4);
        result.Should().AllSatisfy(p => p.CategoryId.Should().Be(2));
    }

    [Fact]
    public async Task GetProductsByCategoryAsync_NoMatch_ReturnsEmpty()
    {
        _repoMock.Setup(r => r.GetByCategoryAsync(99))
                 .ReturnsAsync(Enumerable.Empty<Product>());

        var result = await _sut.GetProductsByCategoryAsync(99);

        result.Should().BeEmpty();
    }

    // ── GetCategoriesAsync ────────────────────────────────────────────

    [Fact]
    public async Task GetCategoriesAsync_ReturnsAllCategories()
    {
        _repoMock.Setup(r => r.GetCategoriesAsync())
                 .ReturnsAsync(new[]
                 {
                     new Category { Id = 1, Name = "巧克力", NameEn = "Chocolate", NameJa = "チョコ", NameZhCn = "巧克力" },
                     new Category { Id = 2, Name = "水果",   NameEn = "Fruit",     NameJa = "フルーツ", NameZhCn = "水果" }
                 });

        var result = await _sut.GetCategoriesAsync();

        result.Should().HaveCount(2);
        result.First().NameEn.Should().Be("Chocolate");
    }

    [Fact]
    public async Task GetCategoriesAsync_NullableLanguageFields_FallBackToEmptyString()
    {
        _repoMock.Setup(r => r.GetCategoriesAsync())
                 .ReturnsAsync(new[]
                 {
                     new Category
                     {
                         Id = 1, Name = "測試", NameEn = "Test", NameJa = "テスト", NameZhCn = "测试",
                         NameTh = null, NameKo = null, NameVi = null, NameMs = null
                     }
                 });

        var result = (await _sut.GetCategoriesAsync()).Single();

        result.NameTh.Should().Be(string.Empty);
        result.NameKo.Should().Be(string.Empty);
        result.NameVi.Should().Be(string.Empty);
        result.NameMs.Should().Be(string.Empty);
    }

    // ── MapToDto ─────────────────────────────────────────────────────

    [Fact]
    public async Task MapToDto_NullableProductLanguageFields_FallBackToEmptyString()
    {
        var product = BuildProduct(1);
        product.NameTh = null; product.NameKo = null; product.NameVi = null; product.NameMs = null;
        product.DescriptionTh = null; product.DescriptionKo = null; product.DescriptionVi = null; product.DescriptionMs = null;
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        var dto = await _sut.GetProductByIdAsync(1);

        dto!.NameTh.Should().Be(string.Empty);
        dto.DescriptionMs.Should().Be(string.Empty);
    }

    [Fact]
    public async Task MapToDto_NullCategory_ReturnsEmptyStringForCategoryFields()
    {
        var product = BuildProduct(1);
        product.Category = null;
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        var dto = await _sut.GetProductByIdAsync(1);

        dto!.CategoryName.Should().Be(string.Empty);
        dto.CategoryNameEn.Should().Be(string.Empty);
    }

    [Fact]
    public async Task MapToDto_WithCategory_MapsCategoryNamesCorrectly()
    {
        var product = BuildProduct(1);
        product.Category = new Category
        {
            Id = 1, Name = "巧克力", NameEn = "Chocolate", NameJa = "チョコ", NameZhCn = "巧克力",
            NameTh = "ช็อก", NameKo = "초콜릿", NameVi = "Sô Cô La", NameMs = "Coklat"
        };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        var dto = await _sut.GetProductByIdAsync(1);

        dto!.CategoryName.Should().Be("巧克力");
        dto.CategoryNameEn.Should().Be("Chocolate");
        dto.CategoryNameTh.Should().Be("ช็อก");
    }

    // ── Helpers ──────────────────────────────────────────────────────

    private static Product BuildProduct(int id, int categoryId = 1) => new()
    {
        Id          = id,
        Name        = $"商品{id}",
        NameEn      = $"Product{id}",
        NameJa      = $"商品{id}JA",
        NameZhCn    = $"商品{id}CN",
        NameTh      = $"สินค้า{id}",
        NameKo      = $"상품{id}",
        NameVi      = $"Sản phẩm{id}",
        NameMs      = $"Produk{id}",
        Description = $"描述{id}",
        DescriptionEn = $"Desc{id}",
        DescriptionJa = $"説明{id}",
        DescriptionZhCn = $"描述{id}CN",
        DescriptionTh = $"คำอธิบาย{id}",
        DescriptionKo = $"설명{id}",
        DescriptionVi = $"Mô tả{id}",
        DescriptionMs = $"Penerangan{id}",
        Price       = 100m * id,
        ImageUrl    = $"http://img/{id}",
        CategoryId  = categoryId,
        IsAvailable = true
    };

    private static IEnumerable<Product> BuildProducts(int count, int categoryId = 1)
        => Enumerable.Range(1, count).Select(i => BuildProduct(i, categoryId));
}
