using Microsoft.Data.SqlClient;
using SilsilaSupply.Models;

namespace SilsilaSupply.Services
{
    public class InventoryService : ServiceBase
    {
        public InventoryService(IConfiguration configuration, ILogger<InventoryService> logger)
            : base(configuration, logger)
        {
        }

        public DataResult<List<InventoryItem>> GetAll(bool lowStockOnly = false)
        {
            try
            {
                var items = new List<InventoryItem>();
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT i.InventoryID, i.ProductID, p.Product_Name, i.WarehouseID, w.Name,
                           i.Quantity_Available, i.Reorder_Level, i.LastUpdated
                    FROM Inventory i
                    LEFT JOIN Product p ON i.ProductID = p.ProductID
                    LEFT JOIN Warehouse w ON i.WarehouseID = w.WarehouseID" +
                    (lowStockOnly
                        ? @"
                    WHERE i.Reorder_Level IS NOT NULL AND i.Quantity_Available < i.Reorder_Level"
                        : string.Empty) + @"
                    ORDER BY i.LastUpdated DESC";
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new InventoryItem
                    {
                        InventoryId = reader.GetInt32(0),
                        ProductId = reader.GetInt32OrNull(1),
                        ProductName = reader.GetStringOrNull(2),
                        WarehouseId = reader.GetInt32OrNull(3),
                        WarehouseName = reader.GetStringOrNull(4),
                        Quantity = reader.GetInt32(5),
                        ReorderLevel = reader.GetInt32OrNull(6),
                        LastUpdated = reader.GetDateTimeOrNull(7)
                    });
                }
                return DataResult<List<InventoryItem>>.Ok(items);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load inventory.");
                return DataResult<List<InventoryItem>>.Fail(ResolveErrorMessage(ex));
            }
        }

        public DataResult<InventoryItem> GetById(int inventoryId)
        {
            try
            {
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT i.InventoryID, i.ProductID, p.Product_Name, i.WarehouseID, w.Name,
                           i.Quantity_Available, i.Reorder_Level, i.LastUpdated
                    FROM Inventory i
                    LEFT JOIN Product p ON i.ProductID = p.ProductID
                    LEFT JOIN Warehouse w ON i.WarehouseID = w.WarehouseID
                    WHERE i.InventoryID = @ID";
                command.Parameters.AddWithValue("@ID", inventoryId);
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return DataResult<InventoryItem>.Ok(new InventoryItem
                    {
                        InventoryId = reader.GetInt32(0),
                        ProductId = reader.GetInt32OrNull(1),
                        ProductName = reader.GetStringOrNull(2),
                        WarehouseId = reader.GetInt32OrNull(3),
                        WarehouseName = reader.GetStringOrNull(4),
                        Quantity = reader.GetInt32(5),
                        ReorderLevel = reader.GetInt32OrNull(6),
                        LastUpdated = reader.GetDateTimeOrNull(7)
                    });
                }
                return DataResult<InventoryItem>.Fail("This inventory entry could not be found.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load inventory entry {InventoryId}.", inventoryId);
                return DataResult<InventoryItem>.Fail(ResolveErrorMessage(ex));
            }
        }

        public OperationResult Create(InventoryInput input)
        {
            try
            {
                ExecuteInTransaction((connection, transaction) =>
                {
                    object? existing = ExecuteScalar(connection, transaction,
                        "SELECT InventoryID FROM Inventory WHERE ProductID = @ProductID AND WarehouseID = @WarehouseID",
                        ("@ProductID", input.ProductId),
                        ("@WarehouseID", input.WarehouseId));

                    if (existing != null)
                    {
                        ExecuteNonQuery(connection, transaction,
                            "UPDATE Inventory SET Quantity_Available = Quantity_Available + @Quantity, LastUpdated = GETDATE() " +
                            "WHERE ProductID = @ProductID AND WarehouseID = @WarehouseID",
                            ("@ProductID", input.ProductId),
                            ("@WarehouseID", input.WarehouseId),
                            ("@Quantity", input.Quantity));
                    }
                    else
                    {
                        ExecuteNonQuery(connection, transaction,
                            "INSERT INTO Inventory (ProductID, WarehouseID, Quantity_Available, LastUpdated) " +
                            "VALUES (@ProductID, @WarehouseID, @Quantity, GETDATE())",
                            ("@ProductID", input.ProductId),
                            ("@WarehouseID", input.WarehouseId),
                            ("@Quantity", input.Quantity));
                    }
                });
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to record inventory stock.");
                return OperationResult.Fail(
                    ResolveErrorMessage(ex, referencedMessage: "The selected product or warehouse no longer exists."));
            }
        }

        public OperationResult SetQuantity(int inventoryId, int quantity)
        {
            try
            {
                int affected = ExecuteNonQuery(
                    "UPDATE Inventory SET Quantity_Available = @Quantity, LastUpdated = GETDATE() WHERE InventoryID = @ID",
                    ("@Quantity", quantity),
                    ("@ID", inventoryId));
                if (affected == 0)
                {
                    return OperationResult.Fail("This inventory entry no longer exists.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update inventory entry {InventoryId}.", inventoryId);
                return OperationResult.Fail(ResolveErrorMessage(ex));
            }
        }

        public OperationResult Delete(int inventoryId)
        {
            try
            {
                int affected = ExecuteNonQuery("DELETE FROM Inventory WHERE InventoryID = @ID", ("@ID", inventoryId));
                if (affected == 0)
                {
                    return OperationResult.Fail("This inventory entry no longer exists.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete inventory entry {InventoryId}.", inventoryId);
                return OperationResult.Fail(ResolveErrorMessage(ex));
            }
        }
    }
}
