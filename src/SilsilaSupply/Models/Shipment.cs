using System.ComponentModel.DataAnnotations;

namespace SilsilaSupply.Models
{
    public class Shipment
    {
        public int ShipmentId { get; set; }
        public int OrderId { get; set; }
        public string? CustomerName { get; set; }
        public int? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public string? Status { get; set; }
        public string? ShippedVia { get; set; }
    }

    public class ShipmentInput
    {
        [Range(1, int.MaxValue, ErrorMessage = "Select an order.")]
        public int OrderId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a warehouse.")]
        public int WarehouseId { get; set; }

        [StringLength(100, ErrorMessage = "Carrier name cannot exceed 100 characters.")]
        public string? ShippedVia { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string? Status { get; set; }
    }
}
