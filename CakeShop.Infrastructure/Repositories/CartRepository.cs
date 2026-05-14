using CakeShop.Core.Interfaces;
using EC.Entities.Models;
using CakeShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CakeShop.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly CakeShopDbContext _ctx;

    public CartRepository(CakeShopDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<CartItem>> GetBySessionAsync(string sessionId)
        => await _ctx.CartItems
               .Include(i => i.Product)
               .Where(i => i.SessionId == sessionId)
               .ToListAsync();

    public async Task<CartItem?> GetItemAsync(string sessionId, int productId)
        => await _ctx.CartItems
               .FirstOrDefaultAsync(i => i.SessionId == sessionId && i.ProductId == productId);

    public async Task AddItemAsync(CartItem item)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        _ctx.CartItems.Add(item);
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateItemAsync(CartItem item)
    {
        item.UpdatedAt = DateTime.UtcNow;
        _ctx.CartItems.Update(item);
        await _ctx.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(int itemId)
    {
        var item = await _ctx.CartItems.FindAsync(itemId);
        if (item is not null)
        {
            _ctx.CartItems.Remove(item);
            await _ctx.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(string sessionId)
    {
        var items = await _ctx.CartItems.Where(i => i.SessionId == sessionId).ToListAsync();
        _ctx.CartItems.RemoveRange(items);
        await _ctx.SaveChangesAsync();
    }
}
