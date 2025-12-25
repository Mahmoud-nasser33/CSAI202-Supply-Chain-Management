// Handles database data access operations for Supplier.
using Microsoft.Data.SqlClient;
using SupplyChain.Backend.Data;
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public class SupplierRepository : ISupplierRepository
{
    private readonly DatabaseContext _context;

    public SupplierRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Supplier>> GetAllSuppliersAsync()
    {
        List<Supplier> suppliers = new List<Supplier>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"SELECT SupplierID, Name, Contact_Info, Email, Address, LeadTimeDays, Rating FROM Supplier";

        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            suppliers.Add(new Supplier
            {
                SupplierID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Contact_Info = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Address = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                LeadTimeDays = reader.IsDBNull(5) ? 5 : reader.GetInt32(5),
                Rating = reader.IsDBNull(6) ? 0.0m : reader.GetDecimal(6)
            });
        }

        return suppliers;
    }

    public async Task<Supplier?> GetSupplierByIdAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"SELECT SupplierID, Name, Contact_Info, Email, Address, LeadTimeDays, Rating FROM Supplier WHERE SupplierID = @SupplierID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@SupplierID", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Supplier
            {
                SupplierID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Contact_Info = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Address = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                LeadTimeDays = reader.IsDBNull(5) ? 5 : reader.GetInt32(5),
                Rating = reader.IsDBNull(6) ? 0.0m : reader.GetDecimal(6)
            };
        }

        return null;
    }

    public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            INSERT INTO Supplier (Name, Contact_Info, Email, Address, LeadTimeDays, Rating)
            OUTPUT INSERTED.SupplierID
            VALUES (@Name, @Contact_Info, @Email, @Address, @LeadTimeDays, @Rating)";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", supplier.Name);
        command.Parameters.AddWithValue("@Contact_Info", (object?)supplier.Contact_Info ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)supplier.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Address", (object?)supplier.Address ?? DBNull.Value);
        command.Parameters.AddWithValue("@LeadTimeDays", supplier.LeadTimeDays);
        command.Parameters.AddWithValue("@Rating", supplier.Rating);

        int id = (int)await command.ExecuteScalarAsync();
        supplier.SupplierID = id;

        return supplier;
    }

    public async Task<bool> UpdateSupplierAsync(int id, Supplier supplier)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            UPDATE Supplier
            SET Name = @Name,
                Contact_Info = @Contact_Info,
                Email = @Email,
                Address = @Address,
                LeadTimeDays = @LeadTimeDays,
                Rating = @Rating
            WHERE SupplierID = @SupplierID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@SupplierID", id);
        command.Parameters.AddWithValue("@Name", supplier.Name);
        command.Parameters.AddWithValue("@Contact_Info", (object?)supplier.Contact_Info ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)supplier.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Address", (object?)supplier.Address ?? DBNull.Value);
        command.Parameters.AddWithValue("@LeadTimeDays", supplier.LeadTimeDays);
        command.Parameters.AddWithValue("@Rating", supplier.Rating);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"DELETE FROM Supplier WHERE SupplierID = @SupplierID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@SupplierID", id);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}

