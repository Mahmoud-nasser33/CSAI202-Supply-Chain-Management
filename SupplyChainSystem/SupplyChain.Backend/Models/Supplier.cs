// Represents the Supplier entity in the database.
namespace SupplyChain.Backend.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Contact_Info { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int LeadTimeDays { get; set; }
        public decimal Rating { get; set; }
    }
}
