// Handles database data access operations for Product.
using Microsoft.Data.SqlClient;
using SupplyChain.Backend.Data;
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly DatabaseContext _context;

    public ProductRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        List<Product> products = new List<Product>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT p.ProductID, p.Product_Name, p.Description, p.Price, p.CategoryID, c.Name as CategoryName,
                   COALESCE((SELECT SUM(Quantity_Available) FROM Inventory WHERE ProductID = p.ProductID), 0) as StockQuantity,
                   ISNULL(s.Name, 'N/A') as SupplierName, p.SupplierID
            FROM Product p
            LEFT JOIN Category c ON p.CategoryID = c.CategoryID
            LEFT JOIN Supplier s ON p.SupplierID = s.SupplierID
            ORDER BY p.ProductID";

        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            products.Add(new Product
            {
                Product_ID = reader.GetInt32(0),
                Product_Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Price = reader.GetDecimal(3),
                CategoryID = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                CategoryName = reader.IsDBNull(5) ? null : reader.GetString(5),
                StockQuantity = reader.GetInt32(6),
                SupplierName = reader.GetString(7),
                SupplierID = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8)
            });
        }

        return products;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT p.ProductID, p.Product_Name, p.Description, p.Price, p.CategoryID, c.Name as CategoryName,
                   COALESCE((SELECT SUM(Quantity_Available) FROM Inventory WHERE ProductID = p.ProductID), 0) as StockQuantity,
                   ISNULL(s.Name, 'N/A') as SupplierName, p.SupplierID
            FROM Product p
            LEFT JOIN Category c ON p.CategoryID = c.CategoryID
            LEFT JOIN Supplier s ON p.SupplierID = s.SupplierID
            WHERE p.ProductID = @ProductID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ProductID", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Product
            {
                Product_ID = reader.GetInt32(0),
                Product_Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Price = reader.GetDecimal(3),
                CategoryID = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                CategoryName = reader.IsDBNull(5) ? null : reader.GetString(5),
                SupplierName = reader.GetString(7),
                SupplierID = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8)
            };
        }

        return null;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            string query = @"
                INSERT INTO Product (Product_Name, Description, Price, CategoryID, SupplierID)
                OUTPUT INSERTED.ProductID
                VALUES (@Product_Name, @Description, @Price, @CategoryID, @SupplierID)";

            using SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@Product_Name", product.Product_Name);
            command.Parameters.AddWithValue("@Description", (object?)product.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@CategoryID", product.CategoryID > 0 ? (object)product.CategoryID : DBNull.Value);
            command.Parameters.AddWithValue("@SupplierID", product.SupplierID > 0 ? (object)product.SupplierID : DBNull.Value);

            var newId = (int)await command.ExecuteScalarAsync();
            product.Product_ID = newId;

            var invQuery = @"
                INSERT INTO Inventory (ProductID, WarehouseID, Quantity_Available)
                VALUES (@ProductID, @WarehouseID, @Quantity_Available)";

            using SqlCommand invCmd = new SqlCommand(invQuery, connection, transaction);
            invCmd.Parameters.AddWithValue("@ProductID", newId);
            invCmd.Parameters.AddWithValue("@WarehouseID", 1);
            invCmd.Parameters.AddWithValue("@Quantity_Available", product.StockQuantity);
            await invCmd.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return product;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> UpdateProductAsync(int id, Product product)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            UPDATE Product
            SET Product_Name = @Product_Name,
                Description = @Description,
                Price = @Price,
                CategoryID = @CategoryID,
                SupplierID = @SupplierID
            WHERE ProductID = @ProductID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ProductID", id);
        command.Parameters.AddWithValue("@Product_Name", product.Product_Name);
        command.Parameters.AddWithValue("@Description", (object?)product.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@CategoryID", product.CategoryID > 0 ? (object)product.CategoryID : DBNull.Value);
        command.Parameters.AddWithValue("@SupplierID", product.SupplierID > 0 ? (object)product.SupplierID : DBNull.Value);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"DELETE FROM Product WHERE ProductID = @ProductID";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ProductID", id);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        List<Product> products = new List<Product>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT p.ProductID, p.Product_Name, p.Description, p.Price, p.CategoryID, c.Name as CategoryName,
                   COALESCE((SELECT SUM(Quantity_Available) FROM Inventory WHERE ProductID = p.ProductID), 0) as StockQuantity,
                   ISNULL(s.Name, 'N/A') as SupplierName
            FROM Product p
            LEFT JOIN Category c ON p.CategoryID = c.CategoryID
            LEFT JOIN Supplier s ON p.SupplierID = s.SupplierID
            WHERE p.CategoryID = @CategoryID
            ORDER BY p.ProductID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CategoryID", categoryId);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            products.Add(new Product
            {
                Product_ID = reader.GetInt32(0),
                Product_Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Price = reader.GetDecimal(3),
                CategoryID = reader.GetInt32(4),
                CategoryName = reader.IsDBNull(5) ? null : reader.GetString(5),
                StockQuantity = reader.GetInt32(6),
                SupplierName = reader.GetString(7)
            });
        }

        return products;
    }
}

