using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface IProductManagementService
{
    Task<ProductDto>              CreateProductAsync(ProductSaveRequest request, string operatorName);
    Task<ProductDto>              UpdateProductAsync(int id, ProductSaveRequest request, string operatorName);
    Task                          DeleteProductAsync(int id);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
}
