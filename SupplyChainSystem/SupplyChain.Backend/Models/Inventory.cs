// Represents the Inventory entity in the database.
namespace SupplyChain.Backend.Models
{
    public class Inventory
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public int WarehouseID { get; set; }
        public int Quantity_Available { get; set; }
        public int? Reorder_Level { get; set; }
        public string? ProductName { get; set; }
        public string? WarehouseName { get; set; }
    }
}
