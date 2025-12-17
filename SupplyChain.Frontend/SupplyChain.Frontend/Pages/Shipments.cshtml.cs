using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class ShipmentsModel : PageModel
    {
        public List<ShipmentDto> Shipments { get; set; } = new List<ShipmentDto>();

        public IActionResult OnPostDelete(int id)
        {
            // MOCK BEHAVIOR: Simulate Deletion
            // In a real app, we would call the API: await client.DeleteAsync($"api/Shipments/{id}");
            
            // For now, just reload the page to show it "worked" (UI will reset to mock data, but action is handled)
            return RedirectToPage();
        }

        public void OnGet()
        {
            // High-Fidelity Mock Data based on User Request
            Shipments = new List<ShipmentDto>
            {
                new ShipmentDto
                {
                    Id = 1,
                    OriginWarehouse = "Central Hub",
                    DestinationAddress = "Springfield Distribution Center",
                    Status = "In Transit",
                    EstimatedArrival = new DateTime(2025, 12, 19),
                    Progress = 65, // Percentage for progress bar
                    CurrentStep = 2, // 0: Processing, 1: Shipped, 2: In Transit, 3: Delivered
                    Items = new List<ShipmentItemDto>
                    {
                        new ShipmentItemDto { ProductName = "Laptop", Quantity = 1 },
                        new ShipmentItemDto { ProductName = "Wireless Mouse", Quantity = 2 }
                    }
                },
                new ShipmentDto
                {
                    Id = 2,
                    OriginWarehouse = "West Coast Depot",
                    DestinationAddress = "Shelbyville Branch",
                    Status = "Delivered",
                    EstimatedArrival = DateTime.Now.AddDays(-1),
                    Progress = 100,
                    CurrentStep = 3,
                    Items = new List<ShipmentItemDto>
                    {
                        new ShipmentItemDto { ProductName = "4K Monitor", Quantity = 10 }
                    }
                },
                new ShipmentDto
                {
                    Id = 3,
                    OriginWarehouse = "Euro Hub",
                    DestinationAddress = "London Store",
                    Status = "Processing",
                    EstimatedArrival = DateTime.Now.AddDays(5),
                    Progress = 15,
                    CurrentStep = 0,
                    Items = new List<ShipmentItemDto>
                    {
                        new ShipmentItemDto { ProductName = "Gaming Keyboard", Quantity = 50 },
                        new ShipmentItemDto { ProductName = "Headset", Quantity = 50 }
                    }
                }
            };
        }
    }

    public class ShipmentDto
    {
        public int Id { get; set; }
        public string OriginWarehouse { get; set; }
        public string DestinationAddress { get; set; }
        public string Status { get; set; }
        public DateTime EstimatedArrival { get; set; }
        public int Progress { get; set; }
        public int CurrentStep { get; set; }
        public List<ShipmentItemDto> Items { get; set; }
    }

    public class ShipmentItemDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
