// Handles database data access operations for Warehouse.
using Microsoft.Data.SqlClient;
using SupplyChain.Backend.Data;
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public class WarehouseRepository : IWarehouseRepository
{
    private readonly DatabaseContext _context;

    public WarehouseRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Warehouse>> GetAllWarehousesAsync()
    {
        List<Warehouse> warehouses = new List<Warehouse>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"SELECT WarehouseID, Name, Location FROM Warehouse";

        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            warehouses.Add(new Warehouse
            {
                WarehouseID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Location = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
            });
        }

        return warehouses;
    }

    public async Task<Warehouse?> GetWarehouseByIdAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"SELECT WarehouseID, Name, Location FROM Warehouse WHERE WarehouseID = @WarehouseID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@WarehouseID", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Warehouse
            {
                WarehouseID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Location = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
            };
        }

        return null;
    }

    public async Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            INSERT INTO Warehouse (Name, Location)
            OUTPUT INSERTED.WarehouseID
            VALUES (@Name, @Location)";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", warehouse.Name);
        command.Parameters.AddWithValue("@Location", warehouse.Location);

        int id = (int)await command.ExecuteScalarAsync();
        warehouse.WarehouseID = id;

        return warehouse;
    }

    public async Task<bool> UpdateWarehouseAsync(int id, Warehouse warehouse)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"UPDATE Warehouse SET Name = @Name, Location = @Location WHERE WarehouseID = @WarehouseID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@WarehouseID", id);
        command.Parameters.AddWithValue("@Name", warehouse.Name);
        command.Parameters.AddWithValue("@Location", warehouse.Location);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteWarehouseAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"DELETE FROM Warehouse WHERE WarehouseID = @WarehouseID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@WarehouseID", id);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}

