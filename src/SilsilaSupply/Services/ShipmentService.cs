using Microsoft.Data.SqlClient;
using SilsilaSupply.Models;

namespace SilsilaSupply.Services
{
    public class ShipmentService : ServiceBase
    {
        public ShipmentService(IConfiguration configuration, ILogger<ShipmentService> logger)
            : base(configuration, logger)
        {
        }

        public DataResult<List<Shipment>> GetAll()
        {
            try
            {
                var shipments = new List<Shipment>();
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT s.ShipmentID, s.OrderID, c.Name, s.WarehouseID, w.Name, s.Status, s.Shipped_Via
                    FROM Shipment s
                    LEFT JOIN Purchase_Order po ON s.OrderID = po.OrderID
                    LEFT JOIN Customer c ON po.CustomerID = c.CustomerID
                    LEFT JOIN Warehouse w ON s.WarehouseID = w.WarehouseID
                    ORDER BY s.ShipmentID DESC";
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    shipments.Add(new Shipment
                    {
                        ShipmentId = reader.GetInt32(0),
                        OrderId = reader.GetInt32(1),
                        CustomerName = reader.GetStringOrNull(2),
                        WarehouseId = reader.GetInt32OrNull(3),
                        WarehouseName = reader.GetStringOrNull(4),
                        Status = reader.GetStringOrNull(5),
                        ShippedVia = reader.GetStringOrNull(6)
                    });
                }
                return DataResult<List<Shipment>>.Ok(shipments);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load shipments.");
                return DataResult<List<Shipment>>.Fail(ResolveErrorMessage(ex));
            }
        }

        public DataResult<Shipment> GetById(int shipmentId)
        {
            try
            {
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT s.ShipmentID, s.OrderID, c.Name, s.WarehouseID, w.Name, s.Status, s.Shipped_Via
                    FROM Shipment s
                    LEFT JOIN Purchase_Order po ON s.OrderID = po.OrderID
                    LEFT JOIN Customer c ON po.CustomerID = c.CustomerID
                    LEFT JOIN Warehouse w ON s.WarehouseID = w.WarehouseID
                    WHERE s.ShipmentID = @ID";
                command.Parameters.AddWithValue("@ID", shipmentId);
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return DataResult<Shipment>.Ok(new Shipment
                    {
                        ShipmentId = reader.GetInt32(0),
                        OrderId = reader.GetInt32(1),
                        CustomerName = reader.GetStringOrNull(2),
                        WarehouseId = reader.GetInt32OrNull(3),
                        WarehouseName = reader.GetStringOrNull(4),
                        Status = reader.GetStringOrNull(5),
                        ShippedVia = reader.GetStringOrNull(6)
                    });
                }
                return DataResult<Shipment>.Fail("The shipment could not be found.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load shipment {ShipmentId}.", shipmentId);
                return DataResult<Shipment>.Fail(ResolveErrorMessage(ex));
            }
        }

        public OperationResult Create(ShipmentInput input)
        {
            try
            {
                ExecuteNonQuery(
                    "INSERT INTO Shipment (OrderID, WarehouseID, Shipped_Via) VALUES (@OrderID, @WarehouseID, @ShippedVia)",
                    ("@OrderID", input.OrderId),
                    ("@WarehouseID", input.WarehouseId),
                    ("@ShippedVia", input.ShippedVia));
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create shipment.");
                return OperationResult.Fail(
                    ResolveErrorMessage(ex, referencedMessage: "The selected order or warehouse no longer exists."));
            }
        }

        public OperationResult Update(int shipmentId, ShipmentInput input)
        {
            try
            {
                int affected = ExecuteNonQuery(
                    "UPDATE Shipment SET WarehouseID = @WarehouseID, Shipped_Via = @ShippedVia, Status = @Status WHERE ShipmentID = @ID",
                    ("@WarehouseID", input.WarehouseId),
                    ("@ShippedVia", input.ShippedVia),
                    ("@Status", input.Status),
                    ("@ID", shipmentId));
                if (affected == 0)
                {
                    return OperationResult.Fail("The shipment could not be found.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update shipment {ShipmentId}.", shipmentId);
                return OperationResult.Fail(
                    ResolveErrorMessage(ex, referencedMessage: "The selected warehouse no longer exists."));
            }
        }

        public OperationResult Delete(int shipmentId)
        {
            try
            {
                int affected = ExecuteNonQuery("DELETE FROM Shipment WHERE ShipmentID = @ID", ("@ID", shipmentId));
                if (affected == 0)
                {
                    return OperationResult.Fail("The shipment no longer exists.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete shipment {ShipmentId}.", shipmentId);
                return OperationResult.Fail(ResolveErrorMessage(ex));
            }
        }
    }
}
