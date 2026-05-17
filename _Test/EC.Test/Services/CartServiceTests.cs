using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.CommonService.Services;
using EC.Entities.Models;
using FluentAssertions;
using Moq;

namespace EC.Test.Services;

public class CartServiceTests
{
    private readonly Mock<ICartRepository>    _cartRepoMock    = new();
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly CartService              _sut;

    private const string Session = "user_alice";

    public CartServiceTests()
        => _sut = new CartService(_cartRepoMock.Object, _productRepoMock.Object);

    // ── GetCartAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetCartAsync_WithItems_ReturnsCartDtoWithItems()
    {
        var product = BuildProduct(1, 280m);
        _cartRepoMock.Setup(r => r.GetBySessionAsync(Session))
                     .ReturnsAsync(new[] { BuildCartItem(10, Session, product, qty: 2) });

        var cart = await _sut.GetCartAsync(Session);

        cart.SessionId.Should().Be(Session);
        cart.Items.Should().HaveCount(1);
        cart.Items[0].UnitPrice.Should().Be(280m);
        cart.Items[0].Quantity.Should().Be(2);
    }

    [Fact]
    public async Task GetCartAsync_EmptyCart_ReturnsEmptyDto()
    {
        _cartRepoMock.Setup(r => r.GetBySessionAsync(Session))
                     .ReturnsAsync(Enumerable.Empty<CartItem>());

        var cart = await _sut.GetCartAsync(Session);

        cart.Items.Should().BeEmpty();
        cart.Total.Should().Be(0m);
        cart.ItemCount.Should().Be(0);
    }

    [Fact]
    public async Task GetCartAsync_CalculatesTotal_Correctly()
    {
        var p1 = BuildProduct(1, 100m);
        var p2 = BuildProduct(2, 200m);
        _cartRepoMock.Setup(r => r.GetBySessionAsync(Session))
                     .ReturnsAsync(new[]
                     {
                         BuildCartItem(1, Session, p1, qty: 3),  // 300
                         BuildCartItem(2, Session, p2, qty: 1)   // 200
                     });

        var cart = await _sut.GetCartAsync(Session);

        cart.Total.Should().Be(500m);
        cart.ItemCount.Should().Be(4);
    }

    // ── AddToCartAsync ───────────────────────────────────────────────

    [Fact]
    public async Task AddToCartAsync_ProductNotFound_ReturnsFailure()
    {
        _productRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);

        var result = await _sut.AddToCartAsync(new AddToCartRequest
            { SessionId = Session, ProductId = 99, Quantity = 1 });

        result.Success.Should().BeFalse();
        result.Message.Should().Be("商品不存在");
    }

    [Fact]
    public async Task AddToCartAsync_UnavailableProduct_ReturnsFailure()
    {
        var product = BuildProduct(1, 100m, isAvailable: false);
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        var result = await _sut.AddToCartAsync(new AddToCartRequest
            { SessionId = Session, ProductId = 1, Quantity = 1 });

        result.Success.Should().BeFalse();
        result.Message.Should().Be("商品目前無法購買");
    }

    [Fact]
    public async Task AddToCartAsync_NewItem_AddsAndReturnsSuccess()
    {
        var product = BuildProduct(1, 150m);
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
        _cartRepoMock.Setup(r => r.GetItemAsync(Session, 1)).ReturnsAsync((CartItem?)null);
        _cartRepoMock.Setup(r => r.AddItemAsync(It.IsAny<CartItem>())).Returns(Task.CompletedTask);
        _cartRepoMock.Setup(r => r.GetBySessionAsync(Session))
                     .ReturnsAsync(new[] { BuildCartItem(1, Session, product, qty: 1) });

        var result = await _sut.AddToCartAsync(new AddToCartRequest
            { SessionId = Session, ProductId = 1, Quantity = 1 });

        result.Success.Should().BeTrue();
        result.Message.Should().Be("已加入購物車");
        _cartRepoMock.Verify(r => r.AddItemAsync(It.Is<CartItem>(i =>
            i.SessionId == Session && i.ProductId == 1 && i.Quantity == 1)), Times.Once);
    }

    [Fact]
    public async Task AddToCartAsync_ExistingItem_UpdatesQuantity()
    {
        var product = BuildProduct(1, 150m);
        var existing = BuildCartItem(5, Session, product, qty: 2);
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
        _cartRepoMock.Setup(r => r.GetItemAsync(Session, 1)).ReturnsAsync(existing);
        _cartRepoMock.Setup(r => r.UpdateItemAsync(It.IsAny<CartItem>())).Returns(Task.CompletedTask);
        _cartRepoMock.Setup(r => r.GetBySessionAsync(Session))
                     .ReturnsAsync(new[] { BuildCartItem(5, Session, product, qty: 5) });

        var result = await _sut.AddToCartAsync(new AddToCartRequest
            { SessionId = Session, ProductId = 1, Quantity = 3 });

        result.Success.Should().BeTrue();
        _cartRepoMock.Verify(r => r.UpdateItemAsync(It.Is<CartItem>(i => i.Quantity == 5)), Times.Once);
        _cartRepoMock.Verify(r => r.AddItemAsync(It.IsAny<CartItem>()), Times.Never);
    }

    // ── UpdateQuantityAsync ──────────────────────────────────────────

    [Fact]
    public async Task UpdateQuantityAsync_ValidQuantity_UpdatesItem()
    {
        var product = BuildProduct(1, 100m);
        var item = BuildCartItem(7, Session, product, qty: 1);
        _cartRepoMock.Setup(r => r.GetBySessionAsync(Session)).ReturnsAsync(new[] { item });
        _cartRepoMock.Setup(r => r.UpdateItemAsync(It.IsAny<CartItem>())).Returns(Task.CompletedTask);

        var result = await _sut.UpdateQuantityAsync(Session, 7, 4);

        result.Success.Should().BeTrue();
        result.Message.Should().Be("數量已更新");
        _cartRepoMock.Verify(r => r.UpdateItemAsync(It.Is<CartItem>(i => i.Quantity == 4)), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task UpdateQuantityAsync_ZeroOrNegativeQuantity_RemovesItem(int qty)
    {
        _cartRepoMock.Setup(r => r.RemoveItemAsync(3)).Returns(Task.CompletedTask);
        _cartRepoMock.Setup(r => r.GetBySessionAsync(Session))
                     .ReturnsAsync(Enumerable.Empty<CartItem>());

        var result = await _sut.UpdateQuantityAsync(Session, 3, qty);

        result.Success.Should().BeTrue();
        result.Message.Should().Be("商品已移除");
        _cartRepoMock.Verify(r => r.RemoveItemAsync(3), Times.Once);
    }

    [Fact]
    public async Task UpdateQuantityAsync_ItemNotFound_ReturnsFailure()
    {
        _cartRepoMock.Setup(r => r.GetBySessionAsync(Session))
                     .ReturnsAsync(Enumerable.Empty<CartItem>());

        var result = await _sut.UpdateQuantityAsync(Session, 999, 2);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("購物車項目不存在");
    }

    // ── RemoveFromCartAsync ──────────────────────────────────────────

    [Fact]
    public async Task RemoveFromCartAsync_CallsRemoveAndReturnsSuccess()
    {
        _cartRepoMock.Setup(r => r.RemoveItemAsync(8)).Returns(Task.CompletedTask);
        _cartRepoMock.Setup(r => r.GetBySessionAsync(Session))
                     .ReturnsAsync(Enumerable.Empty<CartItem>());

        var result = await _sut.RemoveFromCartAsync(Session, 8);

        result.Success.Should().BeTrue();
        result.Message.Should().Be("商品已移除");
        _cartRepoMock.Verify(r => r.RemoveItemAsync(8), Times.Once);
    }

    // ── ClearCartAsync ───────────────────────────────────────────────

    [Fact]
    public async Task ClearCartAsync_ClearsCartAndReturnsEmptyCart()
    {
        _cartRepoMock.Setup(r => r.ClearCartAsync(Session)).Returns(Task.CompletedTask);

        var result = await _sut.ClearCartAsync(Session);

        result.Success.Should().BeTrue();
        result.Message.Should().Be("購物車已清空");
        result.Cart!.Items.Should().BeEmpty();
        result.Cart.SessionId.Should().Be(Session);
        _cartRepoMock.Verify(r => r.ClearCartAsync(Session), Times.Once);
    }

    // ── Helpers ──────────────────────────────────────────────────────

    private static Product BuildProduct(int id, decimal price, bool isAvailable = true) => new()
    {
        Id          = id,
        Name        = $"商品{id}",
        NameEn      = $"Product{id}",
        NameJa      = $"商品{id}",
        NameZhCn    = $"商品{id}",
        Price       = price,
        ImageUrl    = $"http://img/{id}",
        CategoryId  = 1,
        IsAvailable = isAvailable
    };

    private static CartItem BuildCartItem(int id, string sessionId, Product product, int qty) => new()
    {
        Id        = id,
        SessionId = sessionId,
        ProductId = product.Id,
        Product   = product,
        Quantity  = qty
    };
}
