using CakeShop.Core.Interfaces;
using EC.Entities.Models;
using CakeShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CakeShop.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly CakeShopDbContext _ctx;

    public ProductRepository(CakeShopDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await _ctx.Products.Include(p => p.Category).AsNoTracking().ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await _ctx.Products.Include(p => p.Category).AsNoTracking()
               .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        => await _ctx.Products.Include(p => p.Category).AsNoTracking()
               .Where(p => p.CategoryId == categoryId).ToListAsync();

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
        => await _ctx.Categories.AsNoTracking().ToListAsync();

    public async Task<Product> CreateAsync(Product product)
    {
        _ctx.Products.Add(product);
        await _ctx.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _ctx.Products.Update(product);
        await _ctx.SaveChangesAsync();
        return await _ctx.Products.Include(p => p.Category)
                         .AsNoTracking()
                         .FirstAsync(p => p.Id == product.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _ctx.Products.FindAsync(id);
        if (product is not null)
        {
            _ctx.Products.Remove(product);
            await _ctx.SaveChangesAsync();
        }
    }
}
