using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using CakeShop.Core.Models;

namespace CakeShop.Business.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartDto> GetCartAsync(string sessionId)
    {
        var items = await _cartRepository.GetBySessionAsync(sessionId);
        return new CartDto
        {
            SessionId = sessionId,
            Items = items.Select(MapToDto).ToList()
        };
    }

    public async Task<CartOperationResponse> AddToCartAsync(AddToCartRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            return Fail("商品不存在");

        if (!product.IsAvailable)
            return Fail("商品目前無法購買");

        var existing = await _cartRepository.GetItemAsync(request.SessionId, request.ProductId);
        if (existing is not null)
        {
            existing.Quantity += request.Quantity;
            await _cartRepository.UpdateItemAsync(existing);
        }
        else
        {
            await _cartRepository.AddItemAsync(new CartItem
            {
                SessionId = request.SessionId,
                ProductId = request.ProductId,
                Product = product,
                Quantity = request.Quantity
            });
        }

        return await OkCart(request.SessionId, "已加入購物車");
    }

    public async Task<CartOperationResponse> UpdateQuantityAsync(string sessionId, int itemId, int quantity)
    {
        if (quantity <= 0)
        {
            await _cartRepository.RemoveItemAsync(itemId);
            return await OkCart(sessionId, "商品已移除");
        }

        var items = await _cartRepository.GetBySessionAsync(sessionId);
        var item = items.FirstOrDefault(i => i.Id == itemId);
        if (item is null)
            return Fail("購物車項目不存在");

        item.Quantity = quantity;
        await _cartRepository.UpdateItemAsync(item);
        return await OkCart(sessionId, "數量已更新");
    }

    public async Task<CartOperationResponse> RemoveFromCartAsync(string sessionId, int itemId)
    {
        await _cartRepository.RemoveItemAsync(itemId);
        return await OkCart(sessionId, "商品已移除");
    }

    public async Task<CartOperationResponse> ClearCartAsync(string sessionId)
    {
        await _cartRepository.ClearCartAsync(sessionId);
        return new CartOperationResponse
        {
            Success = true,
            Message = "購物車已清空",
            Cart = new CartDto { SessionId = sessionId }
        };
    }

    private async Task<CartOperationResponse> OkCart(string sessionId, string message)
    {
        var cart = await GetCartAsync(sessionId);
        return new CartOperationResponse { Success = true, Message = message, Cart = cart };
    }

    private static CartOperationResponse Fail(string message)
        => new() { Success = false, Message = message };

    private static CartItemDto MapToDto(CartItem item) => new()
    {
        Id = item.Id,
        ProductId = item.ProductId,
        ProductName = item.Product?.Name ?? string.Empty,
        ProductNameEn = item.Product?.NameEn ?? string.Empty,
        ProductNameJa = item.Product?.NameJa ?? string.Empty,
        ProductNameZhCn = item.Product?.NameZhCn ?? string.Empty,
        ImageUrl = item.Product?.ImageUrl ?? string.Empty,
        UnitPrice = item.Product?.Price ?? 0,
        Quantity = item.Quantity
    };
}
