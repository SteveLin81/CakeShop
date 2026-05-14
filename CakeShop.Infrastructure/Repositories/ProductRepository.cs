using CakeShop.Core.Interfaces;
using CakeShop.Core.Models;
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
}
