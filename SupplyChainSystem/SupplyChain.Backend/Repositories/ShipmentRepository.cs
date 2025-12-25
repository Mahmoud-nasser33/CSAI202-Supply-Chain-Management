// Handles database data access operations for Shipment.
using Microsoft.Data.SqlClient;
using SupplyChain.Backend.Data;
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public class ShipmentRepository : IShipmentRepository
{
    private readonly DatabaseContext _context;

    public ShipmentRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Shipment>> GetAllShipmentsAsync()
    {
        List<Shipment> shipments = new List<Shipment>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT s.ShipmentID, s.OrderID, s.WarehouseID, s.Shipment_Date, s.Status, s.Shipped_Via,
                   w.Name as WarehouseName, c.Name as CustomerName
            FROM Shipment s
            LEFT JOIN Warehouse w ON s.WarehouseID = w.WarehouseID
            LEFT JOIN Purchase_Order o ON s.OrderID = o.OrderID
            LEFT JOIN Customer c ON o.CustomerID = c.CustomerID";

        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            shipments.Add(new Shipment
            {
                ShipmentID = reader.GetInt32(0),
                OrderID = reader.GetInt32(1),
                WarehouseID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                Shipment_Date = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                Status = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Shipped_Via = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                WarehouseName = reader.IsDBNull(6) ? "N/A" : reader.GetString(6),
                CustomerName = reader.IsDBNull(7) ? "N/A" : reader.GetString(7)
            });
        }

        return shipments;
    }

    public async Task<Shipment?> GetShipmentByIdAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"SELECT ShipmentID, OrderID, WarehouseID, Shipment_Date, Status, Shipped_Via FROM Shipment WHERE ShipmentID = @ShipmentID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ShipmentID", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Shipment
            {
                ShipmentID = reader.GetInt32(0),
                OrderID = reader.GetInt32(1),
                WarehouseID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                Shipment_Date = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                Status = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Shipped_Via = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
            };
        }

        return null;
    }

    public async Task<Shipment> CreateShipmentAsync(Shipment shipment)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            INSERT INTO Shipment (OrderID, WarehouseID, Shipment_Date, Status, Shipped_Via)
            OUTPUT INSERTED.ShipmentID
            VALUES (@OrderID, @WarehouseID, @Shipment_Date, @Status, @Shipped_Via)";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@OrderID", shipment.OrderID);
        command.Parameters.AddWithValue("@WarehouseID", shipment.WarehouseID);
        command.Parameters.AddWithValue("@Shipment_Date", (object?)shipment.Shipment_Date ?? DBNull.Value);
        command.Parameters.AddWithValue("@Status", (object?)shipment.Status ?? DBNull.Value);
        command.Parameters.AddWithValue("@Shipped_Via", (object?)shipment.Shipped_Via ?? DBNull.Value);

        int id = (int)await command.ExecuteScalarAsync();
        shipment.ShipmentID = id;

        return shipment;
    }

    public async Task<bool> UpdateShipmentAsync(int id, Shipment shipment)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"UPDATE Shipment SET OrderID = @OrderID, WarehouseID = @WarehouseID, Shipment_Date = @Shipment_Date, Status = @Status, Shipped_Via = @Shipped_Via WHERE ShipmentID = @ShipmentID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ShipmentID", id);
        command.Parameters.AddWithValue("@OrderID", shipment.OrderID);
        command.Parameters.AddWithValue("@WarehouseID", shipment.WarehouseID);
        command.Parameters.AddWithValue("@Shipment_Date", (object?)shipment.Shipment_Date ?? DBNull.Value);
        command.Parameters.AddWithValue("@Status", (object?)shipment.Status ?? DBNull.Value);
        command.Parameters.AddWithValue("@Shipped_Via", (object?)shipment.Shipped_Via ?? DBNull.Value);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteShipmentAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"DELETE FROM Shipment WHERE ShipmentID = @ShipmentID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ShipmentID", id);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}

