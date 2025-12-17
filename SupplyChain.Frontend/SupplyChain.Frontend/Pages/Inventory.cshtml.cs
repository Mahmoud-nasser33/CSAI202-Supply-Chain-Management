using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class InventoryModel : PageModel
    {
        public List<InventoryItem> InventoryList { get; set; } = new();

        public IActionResult OnPostDelete(int id)
        {
            // MOCK BEHAVIOR: Simulate Deletion
            return RedirectToPage();
        }

        public void OnGet()
        {
            // Mock Data based on Schema
            InventoryList = new List<InventoryItem>
            {
                new InventoryItem { InventoryID = 1, ProductName = "Laptop", WarehouseName = "Main Warehouse", QuantityAvailable = 50, ReorderLevel = 10 },
                new InventoryItem { InventoryID = 2, ProductName = "Smartphone", WarehouseName = "Main Warehouse", QuantityAvailable = 150, ReorderLevel = 20 },
                new InventoryItem { InventoryID = 3, ProductName = "Headphones", WarehouseName = "Main Warehouse", QuantityAvailable = 8, ReorderLevel = 15 }, // Low Stock
                new InventoryItem { InventoryID = 4, ProductName = "Monitor", WarehouseName = "East Side Depot", QuantityAvailable = 20, ReorderLevel = 5 },
                new InventoryItem { InventoryID = 5, ProductName = "Keyboard", WarehouseName = "East Side Depot", QuantityAvailable = 0, ReorderLevel = 10 }  // Out of Stock
            };
        }

        public class InventoryItem
        {
            public int InventoryID { get; set; }
            public string ProductName { get; set; } // Mocked join
            public string WarehouseName { get; set; } // Mocked join
            public int QuantityAvailable { get; set; }
            public int ReorderLevel { get; set; }
        }
    }
}
