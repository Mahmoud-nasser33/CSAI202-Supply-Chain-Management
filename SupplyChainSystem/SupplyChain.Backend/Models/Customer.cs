// Represents the Customer entity in the database.
namespace SupplyChain.Backend.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public int TotalOrders { get; set; }
    }
}
