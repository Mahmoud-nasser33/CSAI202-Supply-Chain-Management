// Interface defining database operations for Product.
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
    Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
}

