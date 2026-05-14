using EC.Entities.Models;

namespace CakeShop.Core.Interfaces;

public interface ICartRepository
{
    Task<IEnumerable<CartItem>> GetBySessionAsync(string sessionId);
    Task<CartItem?> GetItemAsync(string sessionId, int productId);
    Task AddItemAsync(CartItem item);
    Task UpdateItemAsync(CartItem item);
    Task RemoveItemAsync(int itemId);
    Task ClearCartAsync(string sessionId);
}
