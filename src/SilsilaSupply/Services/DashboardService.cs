using Microsoft.Data.SqlClient;
using SilsilaSupply.Models;

namespace SilsilaSupply.Services
{
    public class DashboardService : ServiceBase
    {
        public DashboardService(IConfiguration configuration, ILogger<DashboardService> logger)
            : base(configuration, logger)
        {
        }

        public DataResult<DashboardStats> GetStats()
        {
            try
            {
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT
                        (SELECT COUNT(*) FROM Product) AS TotalProducts,
                        (SELECT COUNT(*) FROM Customer) AS TotalCustomers,
                        (SELECT COUNT(*) FROM Purchase_Order) AS TotalOrders,
                        (SELECT COUNT(*) FROM Inventory
                            WHERE Reorder_Level IS NOT NULL AND Quantity_Available < Reorder_Level) AS LowStockItems,
                        (SELECT COUNT(*) FROM Purchase_Order
                            WHERE Status LIKE '%Gari%' OR Status LIKE '%Mo3alaq%' OR Status LIKE '%Processing%'
                               OR Status LIKE '%Progress%' OR Status LIKE '%Shipped%' OR Status LIKE '%Transit%'
                               OR Status LIKE '%Pending%' OR Status LIKE '%On hold%') AS OrdersInPrep,
                        (SELECT COUNT(*) FROM Purchase_Order
                            WHERE Status LIKE '%Tam%' OR Status LIKE '%Wasel%' OR Status LIKE '%Delivered%'
                               OR Status LIKE '%Completed%' OR Status LIKE '%Done%' OR Status LIKE '%Arrived%') AS OrdersDelivered,
                        (SELECT COUNT(*) FROM Purchase_Order
                            WHERE Status LIKE '%Cancel%' OR Status LIKE '%Failed%' OR Status LIKE '%Rejected%'
                               OR Status LIKE '%Returned%') AS OrdersProblem,
                        (SELECT COUNT(*) FROM Purchase_Order
                            WHERE Status IS NOT NULL
                              AND Status NOT LIKE '%Gari%' AND Status NOT LIKE '%Mo3alaq%' AND Status NOT LIKE '%Processing%'
                              AND Status NOT LIKE '%Progress%' AND Status NOT LIKE '%Shipped%' AND Status NOT LIKE '%Transit%'
                              AND Status NOT LIKE '%Pending%' AND Status NOT LIKE '%On hold%'
                              AND Status NOT LIKE '%Tam%' AND Status NOT LIKE '%Wasel%' AND Status NOT LIKE '%Delivered%'
                              AND Status NOT LIKE '%Completed%' AND Status NOT LIKE '%Done%' AND Status NOT LIKE '%Arrived%'
                              AND Status NOT LIKE '%Cancel%' AND Status NOT LIKE '%Failed%' AND Status NOT LIKE '%Rejected%'
                              AND Status NOT LIKE '%Returned%') AS OrdersOther";
                using SqlDataReader reader = command.ExecuteReader();
                DashboardStats stats = new DashboardStats();
                if (reader.Read())
                {
                    stats.TotalProducts = reader.GetInt32(0);
                    stats.TotalCustomers = reader.GetInt32(1);
                    stats.TotalOrders = reader.GetInt32(2);
                    stats.LowStockItems = reader.GetInt32(3);
                    stats.OrdersInPrep = reader.GetInt32(4);
                    stats.OrdersDelivered = reader.GetInt32(5);
                    stats.OrdersProblem = reader.GetInt32(6);
                    stats.OrdersOther = reader.GetInt32(7);
                }
                return DataResult<DashboardStats>.Ok(stats);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load dashboard statistics.");
                return DataResult<DashboardStats>.Fail(ResolveErrorMessage(ex));
            }
        }

        public DataResult<List<Order>> GetRecentOrders(int count = 5)
        {
            try
            {
                var orders = new List<Order>();
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT TOP (@Count) po.OrderID, po.CustomerID, c.Name, po.OrderDate, po.Status, po.TotalAmount
                    FROM Purchase_Order po
                    LEFT JOIN Customer c ON po.CustomerID = c.CustomerID
                    ORDER BY po.OrderDate DESC";
                command.Parameters.AddWithValue("@Count", count);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderId = reader.GetInt32(0),
                        CustomerId = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                        CustomerName = reader.IsDBNull(2) ? null : reader.GetString(2),
                        OrderDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                        Status = reader.IsDBNull(4) ? null : reader.GetString(4),
                        TotalAmount = reader.IsDBNull(5) ? null : reader.GetDecimal(5)
                    });
                }
                return DataResult<List<Order>>.Ok(orders);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load recent orders.");
                return DataResult<List<Order>>.Fail(ResolveErrorMessage(ex));
            }
        }
    }
}
