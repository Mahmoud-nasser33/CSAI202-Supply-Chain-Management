// Interface defining database operations for nventory.
using Microsoft.Data.SqlClient;
using SupplyChain.Backend.Data;
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public class InventoryRepository : IInventoryRepository
{
    private readonly DatabaseContext _context;

    public InventoryRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Inventory>> GetAllInventoryAsync()
    {
        List<Inventory> inventorylist = new List<Inventory>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT i.InventoryID, i.ProductID, i.WarehouseID, i.Quantity_Available, i.Reorder_Level,
                   p.Product_Name, w.Name as WarehouseName
            FROM Inventory i
            JOIN Product p ON i.ProductID = p.ProductID
            JOIN Warehouse w ON i.WarehouseID = w.WarehouseID";

        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            inventorylist.Add(new Inventory
            {
                InventoryID = reader.GetInt32(0),
                ProductID = reader.GetInt32(1),
                WarehouseID = reader.GetInt32(2),
                Quantity_Available = reader.GetInt32(3),
                Reorder_Level = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                ProductName = reader.GetString(5),
                WarehouseName = reader.GetString(6)
            });
        }

        return inventorylist;
    }

    public async Task<Inventory?> GetInventoryByIdAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"SELECT InventoryID, ProductID, WarehouseID, Quantity_Available, Reorder_Level FROM Inventory WHERE InventoryID = @InventoryID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@InventoryID", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Inventory
            {
                InventoryID = reader.GetInt32(0),
                ProductID = reader.GetInt32(1),
                WarehouseID = reader.GetInt32(2),
                Quantity_Available = reader.GetInt32(3),
                Reorder_Level = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4)
            };
        }

        return null;
    }

    public async Task<Inventory> CreateInventoryAsync(Inventory inventory)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            INSERT INTO Inventory (ProductID, WarehouseID, Quantity_Available, Reorder_Level)
            OUTPUT INSERTED.InventoryID
            VALUES (@ProductID, @WarehouseID, @Quantity_Available, @Reorder_Level)";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ProductID", inventory.ProductID);
        command.Parameters.AddWithValue("@WarehouseID", inventory.WarehouseID);
        command.Parameters.AddWithValue("@Quantity_Available", inventory.Quantity_Available);
        command.Parameters.AddWithValue("@Reorder_Level", (object?)inventory.Reorder_Level ?? DBNull.Value);

        int id = (int)await command.ExecuteScalarAsync();
        inventory.InventoryID = id;

        return inventory;
    }

    public async Task<bool> UpdateInventoryAsync(int id, Inventory inventory)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"UPDATE Inventory SET ProductID = @ProductID, WarehouseID = @WarehouseID, Quantity_Available = @Quantity_Available, Reorder_Level = @Reorder_Level WHERE InventoryID = @InventoryID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@InventoryID", id);
        command.Parameters.AddWithValue("@ProductID", inventory.ProductID);
        command.Parameters.AddWithValue("@WarehouseID", inventory.WarehouseID);
        command.Parameters.AddWithValue("@Quantity_Available", inventory.Quantity_Available);
        command.Parameters.AddWithValue("@Reorder_Level", (object?)inventory.Reorder_Level ?? DBNull.Value);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteInventoryAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"DELETE FROM Inventory WHERE InventoryID = @InventoryID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@InventoryID", id);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}

