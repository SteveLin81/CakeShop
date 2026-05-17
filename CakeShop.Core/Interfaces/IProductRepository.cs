using EC.Entities.Models;

namespace CakeShop.Core.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Category?>            GetCategoryByIdAsync(int id);
    Task<Category>             CreateCategoryAsync(Category category);
    Task<Category>             UpdateCategoryAsync(Category category);
    Task                       DeleteCategoryAsync(int id);
    Task<Product>  CreateAsync(Product product);
    Task<Product>  UpdateAsync(Product product);
    Task           DeleteAsync(int id);
}
