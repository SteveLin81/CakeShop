using EC.Entities.Models;

namespace CakeShop.Core.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Product>  CreateAsync(Product product);
    Task<Product>  UpdateAsync(Product product);
    Task           DeleteAsync(int id);
}
