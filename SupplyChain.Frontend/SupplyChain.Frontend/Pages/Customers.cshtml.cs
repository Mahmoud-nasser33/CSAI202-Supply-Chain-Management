using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class CustomersModel : PageModel
    {
        public List<CustomerItem> CustomerList { get; set; } = new();

        public Microsoft.AspNetCore.Mvc.IActionResult OnPostDelete(int id)
        {
            // MOCK BEHAVIOR: Simulate Deletion
            return RedirectToPage();
        }

        public void OnGet()
        {
            // Mock Data
            CustomerList = new List<CustomerItem>
            {
                new CustomerItem { CustomerID = 1, Name = "Alice Johnson", Email = "alice@example.com", Address = "123 Main St, New York, NY", TotalOrders = 5 },
                new CustomerItem { CustomerID = 2, Name = "Bob Smith", Email = "bob@example.com", Address = "456 Market St, San Francisco, CA", TotalOrders = 2 },
                new CustomerItem { CustomerID = 3, Name = "Charlie Brown", Email = "charlie@example.com", Address = "789 Broadway, Seattle, WA", TotalOrders = 12 }
            };
        }

        public class CustomerItem
        {
            public int CustomerID { get; set; }
            public string Name { get; set; }
            public string Email { get; set; } // From User table join
            public string Address { get; set; }
            public int TotalOrders { get; set; } // Analytic metric
        }
    }
}
