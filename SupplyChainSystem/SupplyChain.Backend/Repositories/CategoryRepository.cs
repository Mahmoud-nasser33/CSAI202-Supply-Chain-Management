// Handles database data access operations for Category.
using Microsoft.Data.SqlClient;
using SupplyChain.Backend.Data;
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly DatabaseContext _context;

    public CategoryRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        List<Category> categories = new List<Category>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"SELECT CategoryID, Name FROM Category ORDER BY Name";
        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            categories.Add(new Category
            {
                CategoryID = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return categories;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"SELECT CategoryID, Name FROM Category WHERE CategoryID = @CategoryID";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CategoryID", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Category
            {
                CategoryID = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }

        return null;
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"INSERT INTO Category (Name) OUTPUT INSERTED.CategoryID VALUES (@Name)";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", category.Name);

        var newId = (int)await command.ExecuteScalarAsync();
        category.CategoryID = newId;

        return category;
    }

    public async Task<bool> UpdateCategoryAsync(int id, Category category)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"UPDATE Category SET Name = @Name WHERE CategoryID = @CategoryID";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CategoryID", id);
        command.Parameters.AddWithValue("@Name", category.Name);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"DELETE FROM Category WHERE CategoryID = @CategoryID";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CategoryID", id);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}

