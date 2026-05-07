using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface ICartService
{
    Task<CartDto> GetCartAsync(string sessionId);
    Task<CartOperationResponse> AddToCartAsync(AddToCartRequest request);
    Task<CartOperationResponse> UpdateQuantityAsync(string sessionId, int itemId, int quantity);
    Task<CartOperationResponse> RemoveFromCartAsync(string sessionId, int itemId);
    Task<CartOperationResponse> ClearCartAsync(string sessionId);
}
