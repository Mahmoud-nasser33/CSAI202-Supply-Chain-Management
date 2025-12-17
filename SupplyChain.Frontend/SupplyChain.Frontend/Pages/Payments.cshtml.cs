using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class PaymentsModel : PageModel
    {
        public List<PaymentItem> PaymentList { get; set; } = new();

        public void OnGet()
        {
            // Mock Data
            PaymentList = new List<PaymentItem>
            {
                new PaymentItem { PaymentID = 101, OrderID = 1, PaymentDate = DateTime.Now.AddDays(-5), PaymentMethod = "Credit Card", Status = "Completed", Amount = 1200.00m },
                new PaymentItem { PaymentID = 102, OrderID = 2, PaymentDate = DateTime.Now.AddDays(-2), PaymentMethod = "PayPal", Status = "Completed", Amount = 150.00m },
                new PaymentItem { PaymentID = 103, OrderID = 3, PaymentDate = DateTime.Now.AddHours(-1), PaymentMethod = "Bank Transfer", Status = "Pending", Amount = 5000.00m }
            };
        }

        public class PaymentItem
        {
            public int PaymentID { get; set; }
            public int OrderID { get; set; }
            public DateTime PaymentDate { get; set; }
            public string PaymentMethod { get; set; }
            public string Status { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
