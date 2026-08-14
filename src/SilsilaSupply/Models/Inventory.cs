using System.ComponentModel.DataAnnotations;

namespace SilsilaSupply.Models
{
    public class InventoryItem
    {
        public int InventoryId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public int Quantity { get; set; }
        public int? ReorderLevel { get; set; }
        public DateTime? LastUpdated { get; set; }

        public bool IsLowStock =>
            ReorderLevel.HasValue ? Quantity <= ReorderLevel.Value : Quantity < 10;
    }

    public class InventoryInput
    {
        [Range(1, int.MaxValue, ErrorMessage = "Select a product.")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a warehouse.")]
        public int WarehouseId { get; set; }

        [Range(1, 999999, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }
    }

    public class InventoryQuantityInput
    {
        [Range(0, 999999, ErrorMessage = "Quantity must be between 0 and 999,999.")]
        public int Quantity { get; set; }
    }
}
