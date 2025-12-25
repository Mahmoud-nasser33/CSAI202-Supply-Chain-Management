using SupplyChain.Backend.Models;
// Defines the SharedDtos class/logic for the Supply Chain system.
namespace SupplyChain.Frontend.Pages
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int LeadTimeDays { get; set; }
        public double Rating { get; set; }
    }

    public class CategoryDto
    {
        public int CategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ProductInputDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
