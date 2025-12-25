// Handles database data access operations for Order.
using Microsoft.Data.SqlClient;
using SupplyChain.Backend.Data;
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly DatabaseContext _context;

    public OrderRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        List<Order> orders = new List<Order>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT o.OrderID, o.CustomerID, o.OrderDate, o.Status, o.TotalAmount, c.Name as CustomerName
            FROM Purchase_Order o
            LEFT JOIN Customer c ON o.CustomerID = c.CustomerID
            ORDER BY o.OrderDate DESC";

        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            orders.Add(new Order
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                OrderDate = reader.GetDateTime(2),
                Status = reader.GetString(3),
                TotalAmount = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                CustomerName = reader.IsDBNull(5) ? null : reader.GetString(5)
            });
        }

        return orders;
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT o.OrderID, o.CustomerID, o.OrderDate, o.Status, o.TotalAmount, c.Name as CustomerName
            FROM Purchase_Order o
            LEFT JOIN Customer c ON o.CustomerID = c.CustomerID
            WHERE o.OrderID = @OrderID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@OrderID", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Order
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                OrderDate = reader.GetDateTime(2),
                Status = reader.GetString(3),
                TotalAmount = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                CustomerName = reader.IsDBNull(5) ? null : reader.GetString(5)
            };
        }

        return null;
    }

    public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
    {
        List<Order> orders = new List<Order>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT o.OrderID, o.CustomerID, o.OrderDate, o.Status, o.TotalAmount, c.Name as CustomerName
            FROM Purchase_Order o
            LEFT JOIN Customer c ON o.CustomerID = c.CustomerID
            WHERE o.CustomerID = @CustomerID
            ORDER BY o.OrderDate DESC";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CustomerID", customerId);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            orders.Add(new Order
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                OrderDate = reader.GetDateTime(2),
                Status = reader.GetString(3),
                TotalAmount = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                CustomerName = reader.IsDBNull(5) ? null : reader.GetString(5)
            });
        }

        return orders;
    }

    public async Task<Order> CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {

            var orderQuery = @"
                INSERT INTO Purchase_Order (CustomerID, OrderDate, Status, TotalAmount)
                OUTPUT INSERTED.OrderID
                VALUES (@CustomerID, @OrderDate, @Status, @TotalAmount)";

            using SqlCommand orderCommand = new SqlCommand(orderQuery, connection, transaction);
            orderCommand.Parameters.AddWithValue("@CustomerID", order.CustomerID);
            orderCommand.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            orderCommand.Parameters.AddWithValue("@Status", order.Status);
            orderCommand.Parameters.AddWithValue("@TotalAmount", (object?)order.TotalAmount ?? DBNull.Value);

            var orderId = (int)await orderCommand.ExecuteScalarAsync();
            order.OrderID = orderId;

            foreach (var detail in orderDetails)
            {
                var detailQuery = @"
                    INSERT INTO Order_Details (OrderID, ProductID, Quantity, UnitPrice)
                    VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice)";

                using SqlCommand detailCommand = new SqlCommand(detailQuery, connection, transaction);
                detailCommand.Parameters.AddWithValue("@OrderID", orderId);
                detailCommand.Parameters.AddWithValue("@ProductID", detail.ProductID);
                detailCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);
                detailCommand.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);

                await detailCommand.ExecuteNonQueryAsync();
            }

            transaction.Commit();
            return order;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> UpdateOrderStatusAsync(int id, string status)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"UPDATE Purchase_Order SET Status = @Status WHERE OrderID = @OrderID";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@OrderID", id);
        command.Parameters.AddWithValue("@Status", status);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {

            var deleteDetailsQuery = "DELETE FROM Order_Details WHERE OrderID = @OrderID";
            using SqlCommand deleteDetailsCommand = new SqlCommand(deleteDetailsQuery, connection, transaction);
            deleteDetailsCommand.Parameters.AddWithValue("@OrderID", id);
            await deleteDetailsCommand.ExecuteNonQueryAsync();

            var deleteOrderQuery = "DELETE FROM Purchase_Order WHERE OrderID = @OrderID";
            using SqlCommand deleteOrderCommand = new SqlCommand(deleteOrderQuery, connection, transaction);
            deleteOrderCommand.Parameters.AddWithValue("@OrderID", id);
            int rowsAffected = await deleteOrderCommand.ExecuteNonQueryAsync();

            transaction.Commit();
            return rowsAffected > 0;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<List<OrderDetail>> GetOrderDetailsAsync(int orderId)
    {
        List<OrderDetail> details = new List<OrderDetail>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT od.OrderID, od.ProductID, od.Quantity, od.UnitPrice, p.Product_Name as ProductName
            FROM Order_Details od
            JOIN Product p ON od.ProductID = p.ProductID
            WHERE od.OrderID = @OrderID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@OrderID", orderId);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            details.Add(new OrderDetail
            {
                OrderID = reader.GetInt32(0),
                ProductID = reader.GetInt32(1),
                Quantity = reader.GetInt32(2),
                UnitPrice = reader.GetDecimal(3),
                ProductName = reader.IsDBNull(4) ? null : reader.GetString(4)
            });
        }

        return details;
    }
}

