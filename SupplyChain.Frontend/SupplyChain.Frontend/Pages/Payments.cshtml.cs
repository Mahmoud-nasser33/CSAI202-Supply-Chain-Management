// Defines the Payments.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class PaymentsModel : PageModel
    {
        public List<PaymentItem> PaymentList { get; set; } = new();

        public void OnGet()
        {
            // Empty until backend API is implemented.
            PaymentList = new List<PaymentItem>();
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
