using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SupplyChain.Frontend.Pages
{
    public class OrdersModel : PageModel
    {
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
        public string SearchString { get; set; }

        public IActionResult OnPostDelete(int id)
        {
            // MOCK BEHAVIOR: Simulate Cancellation/Deletion
            return RedirectToPage();
        }

        public void OnGet(string searchString)
        {
            SearchString = searchString;

            // Mock Data
            var allOrders = new List<OrderDto>
            {
                new OrderDto { OrderID = 101, OrderDate = new DateTime(2025, 12, 10), Status = "Shipped", TotalAmount = 1500.00m },
                new OrderDto { OrderID = 102, OrderDate = new DateTime(2025, 12, 12), Status = "Processing", TotalAmount = 450.50m },
                new OrderDto { OrderID = 103, OrderDate = new DateTime(2025, 12, 14), Status = "Delivered", TotalAmount = 2100.00m },
                new OrderDto { OrderID = 104, OrderDate = new DateTime(2025, 12, 15), Status = "Pending", TotalAmount = 120.00m },
                new OrderDto { OrderID = 105, OrderDate = new DateTime(2025, 12, 16), Status = "Cancelled", TotalAmount = 0.00m }
            };

            if (!string.IsNullOrEmpty(searchString))
            {
                Orders = allOrders.Where(o => 
                    o.OrderID.ToString().Contains(searchString) || 
                    o.Status.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                Orders = allOrders;
            }
        }
    }

    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
