// Represents the Shipment entity in the database.
namespace SupplyChain.Backend.Models
{
    public class Shipment
    {
        public int ShipmentID { get; set; }
        public int OrderID { get; set; }
        public int WarehouseID { get; set; }
        public DateTime? Shipment_Date { get; set; }
        public string? Status { get; set; }
        public string? Shipped_Via { get; set; }
        public string? WarehouseName { get; set; }
        public string? CustomerName { get; set; }
    }
}
