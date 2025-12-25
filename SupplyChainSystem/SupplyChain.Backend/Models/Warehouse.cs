// Represents the Warehouse entity in the database.
namespace SupplyChain.Backend.Models
{
    public class Warehouse
    {
        public int WarehouseID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
