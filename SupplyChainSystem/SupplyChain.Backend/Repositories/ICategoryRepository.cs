// Interface defining database operations for Category.
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category> CreateCategoryAsync(Category category);
    Task<bool> UpdateCategoryAsync(int id, Category category);
    Task<bool> DeleteCategoryAsync(int id);
}

