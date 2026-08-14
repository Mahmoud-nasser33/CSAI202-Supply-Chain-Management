using Microsoft.Data.SqlClient;
using SilsilaSupply.Models;

namespace SilsilaSupply.Services
{
    public class OrderService : ServiceBase
    {
        public OrderService(IConfiguration configuration, ILogger<OrderService> logger)
            : base(configuration, logger)
        {
        }

        public DataResult<List<Order>> GetAll()
        {
            try
            {
                var orders = new List<Order>();
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT po.OrderID, po.CustomerID, c.Name, po.OrderDate, po.Status, po.TotalAmount
                    FROM Purchase_Order po
                    LEFT JOIN Customer c ON po.CustomerID = c.CustomerID
                    ORDER BY po.OrderID DESC";
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderId = reader.GetInt32(0),
                        CustomerId = reader.GetInt32OrNull(1),
                        CustomerName = reader.GetStringOrNull(2),
                        OrderDate = reader.GetDateTimeOrNull(3),
                        Status = reader.GetStringOrNull(4),
                        TotalAmount = reader.GetDecimalOrNull(5)
                    });
                }
                return DataResult<List<Order>>.Ok(orders);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load orders.");
                return DataResult<List<Order>>.Fail(ResolveErrorMessage(ex));
            }
        }

        public DataResult<Order> GetById(int orderId)
        {
            try
            {
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT po.OrderID, po.CustomerID, c.Name, po.OrderDate, po.Status, po.TotalAmount
                    FROM Purchase_Order po
                    LEFT JOIN Customer c ON po.CustomerID = c.CustomerID
                    WHERE po.OrderID = @ID";
                command.Parameters.AddWithValue("@ID", orderId);
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return DataResult<Order>.Ok(new Order
                    {
                        OrderId = reader.GetInt32(0),
                        CustomerId = reader.GetInt32OrNull(1),
                        CustomerName = reader.GetStringOrNull(2),
                        OrderDate = reader.GetDateTimeOrNull(3),
                        Status = reader.GetStringOrNull(4),
                        TotalAmount = reader.GetDecimalOrNull(5)
                    });
                }
                return DataResult<Order>.Fail("The order could not be found.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load order {OrderId}.", orderId);
                return DataResult<Order>.Fail(ResolveErrorMessage(ex));
            }
        }

        public OperationResult Create(OrderInput input)
        {
            try
            {
                ExecuteNonQuery(
                    "INSERT INTO Purchase_Order (CustomerID, TotalAmount) VALUES (@CustomerID, @TotalAmount)",
                    ("@CustomerID", input.CustomerId),
                    ("@TotalAmount", input.TotalAmount));
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create order.");
                return OperationResult.Fail(
                    ResolveErrorMessage(ex, referencedMessage: "The selected customer no longer exists."));
            }
        }

        public OperationResult Update(int orderId, OrderInput input)
        {
            try
            {
                int affected = ExecuteNonQuery(
                    "UPDATE Purchase_Order SET CustomerID = @CustomerID, TotalAmount = @TotalAmount, Status = @Status WHERE OrderID = @ID",
                    ("@CustomerID", input.CustomerId),
                    ("@TotalAmount", input.TotalAmount),
                    ("@Status", input.Status),
                    ("@ID", orderId));
                if (affected == 0)
                {
                    return OperationResult.Fail("The order could not be found.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update order {OrderId}.", orderId);
                return OperationResult.Fail(
                    ResolveErrorMessage(ex, referencedMessage: "The selected customer no longer exists."));
            }
        }

        public OperationResult Delete(int orderId)
        {
            try
            {
                int affected = ExecuteInTransaction((connection, transaction) =>
                {
                    ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Order_Details WHERE OrderID = @ID", ("@ID", orderId));
                    ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Shipment WHERE OrderID = @ID", ("@ID", orderId));
                    ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Payment WHERE OrderID = @ID", ("@ID", orderId));
                    return ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Purchase_Order WHERE OrderID = @ID", ("@ID", orderId));
                });
                if (affected == 0)
                {
                    return OperationResult.Fail("The order no longer exists.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete order {OrderId}.", orderId);
                return OperationResult.Fail(ResolveErrorMessage(ex));
            }
        }
    }
}
