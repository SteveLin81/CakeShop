using CakeShop.Core.Interfaces;
using CakeShop.Core.Models;

namespace CakeShop.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly List<CartItem> _items = new();
    private int _nextId = 1;

    public Task<IEnumerable<CartItem>> GetBySessionAsync(string sessionId)
        => Task.FromResult(_items.Where(i => i.SessionId == sessionId));

    public Task<CartItem?> GetItemAsync(string sessionId, int productId)
        => Task.FromResult(_items.FirstOrDefault(i =>
            i.SessionId == sessionId && i.ProductId == productId));

    public Task AddItemAsync(CartItem item)
    {
        item.Id = _nextId++;
        _items.Add(item);
        return Task.CompletedTask;
    }

    public Task UpdateItemAsync(CartItem item)
    {
        var existing = _items.FirstOrDefault(i => i.Id == item.Id);
        if (existing is not null)
            existing.Quantity = item.Quantity;
        return Task.CompletedTask;
    }

    public Task RemoveItemAsync(int itemId)
    {
        _items.RemoveAll(i => i.Id == itemId);
        return Task.CompletedTask;
    }

    public Task ClearCartAsync(string sessionId)
    {
        _items.RemoveAll(i => i.SessionId == sessionId);
        return Task.CompletedTask;
    }
}
