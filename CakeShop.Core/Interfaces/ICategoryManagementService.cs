using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface ICategoryManagementService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto?>             GetByIdAsync(int id);
    Task<CategoryDto>              CreateAsync(CategorySaveRequest req, string operatorName);
    Task<CategoryDto>              UpdateAsync(int id, CategorySaveRequest req, string operatorName);
    Task                           DeleteAsync(int id);
}
