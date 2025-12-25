// Manages HTTP requests and API logic for DashboardSummary.
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SupplyChain.Backend.Data;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardSummaryController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public DashboardSummaryController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                using SqlConnection connection = _context.CreateConnection();
                await connection.OpenAsync();

                string kpiQuery = @"
                    SELECT
                        (SELECT COUNT(*) FROM Product) as ProductCount,
                        (SELECT COUNT(*) FROM Purchase_Order) as OrderCount,
                        (SELECT ISNULL(SUM(TotalAmount), 0) FROM Purchase_Order) as TotalRevenue,
                        (SELECT COUNT(*) FROM Inventory WHERE Quantity_Available <= Reorder_Level) as LowStockCount";

                using SqlCommand kpiCommand = new SqlCommand(kpiQuery, connection);
                using SqlDataReader reader = await kpiCommand.ExecuteReaderAsync();

                DashboardSummary summary = new();
                if (await reader.ReadAsync())
                {
                    summary.ProductCount = reader.GetInt32(0);
                    summary.OrderCount = reader.GetInt32(1);
                    summary.TotalRevenue = reader.GetDecimal(2);
                    summary.LowStockCount = reader.GetInt32(3);
                }
                await reader.CloseAsync();

                string activityQuery = @"
                    SELECT TOP 5 'New Order' as Type, 'Order #' + CAST(OrderID as NVARCHAR) as Description, OrderDate as Date
                    FROM Purchase_Order
                    ORDER BY OrderDate DESC";

                using SqlCommand activityCommand = new SqlCommand(activityQuery, connection);
                using var activityReader = await activityCommand.ExecuteReaderAsync();
                while (await activityReader.ReadAsync())
                {
                    summary.RecentActivity.Add(new ActivityItem {
                        Type = activityReader.GetString(0),
                        Description = activityReader.GetString(1),
                        Date = activityReader.GetDateTime(2)
                    });
                }

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching dashboard summary", error = ex.Message });
            }
        }

        [HttpGet("analytics")]
        public async Task<IActionResult> GetAnalytics()
        {
            try
            {
                using SqlConnection connection = _context.CreateConnection();
                await connection.OpenAsync();

                AnalyticsData data = new();

                string revQuery = @"
                    SELECT FORMAT(OrderDate, 'MMM') as Month, SUM(TotalAmount) as Revenue
                    FROM Purchase_Order
                    WHERE OrderDate >= DATEADD(month, -6, GETDATE())
                    GROUP BY FORMAT(OrderDate, 'MMM'), MONTH(OrderDate)
                    ORDER BY MONTH(OrderDate)";

                using SqlCommand revCmd = new SqlCommand(revQuery, connection);
                using var revReader = await revCmd.ExecuteReaderAsync();
                while (await revReader.ReadAsync())
                {
                    data.RevenueLabels.Add(revReader.GetString(0));
                    data.RevenueValues.Add(revReader.GetDecimal(1));
                }
                await revReader.CloseAsync();

                string invQuery = @"
                    SELECT c.Name, SUM(i.Quantity_Available)
                    FROM Inventory i
                    JOIN Product p ON i.ProductID = p.ProductID
                    JOIN Category c ON p.CategoryID = c.CategoryID
                    GROUP BY c.Name";

                using SqlCommand invCmd = new SqlCommand(invQuery, connection);
                using var invReader = await invCmd.ExecuteReaderAsync();
                while (await invReader.ReadAsync())
                {
                    data.CategoryLabels.Add(invReader.GetString(0));
                    data.CategoryValues.Add(invReader.GetInt32(1));
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching analytics", error = ex.Message });
            }
        }
    }

    public class AnalyticsData
    {
        public List<string> RevenueLabels { get; set; } = new();
        public List<decimal> RevenueValues { get; set; } = new();
        public List<string> CategoryLabels { get; set; } = new();
        public List<int> CategoryValues { get; set; } = new();
    }

    public class DashboardSummary
    {
        public int ProductCount { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public int LowStockCount { get; set; }
        public List<ActivityItem> RecentActivity { get; set; } = new();
    }

    public class ActivityItem
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
