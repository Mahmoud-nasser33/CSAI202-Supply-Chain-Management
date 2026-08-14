namespace SilsilaSupply.Models
{
    public class DashboardStats
    {
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public int LowStockItems { get; set; }

        public int OrdersInPrep { get; set; }
        public int OrdersDelivered { get; set; }
        public int OrdersProblem { get; set; }
        public int OrdersOther { get; set; }

        public int OrdersTracked => OrdersInPrep + OrdersDelivered + OrdersProblem + OrdersOther;
    }
}
