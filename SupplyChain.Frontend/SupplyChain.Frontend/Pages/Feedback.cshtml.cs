using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class FeedbackModel : PageModel
    {
        public List<FeedbackItem> FeedbackList { get; set; } = new();

        public void OnGet()
        {
            FeedbackList = new List<FeedbackItem>
            {
                new FeedbackItem { FeedbackID = 1, OrderID = 101, CustomerName = "Alice Johnson", Rating = 5, Comment = "Excellent service and fast delivery!" },
                new FeedbackItem { FeedbackID = 2, OrderID = 104, CustomerName = "Bob Smith", Rating = 3, Comment = "Product was okay, but packaging was damaged." },
                new FeedbackItem { FeedbackID = 3, OrderID = 112, CustomerName = "Charlie Brown", Rating = 1, Comment = "Wrong item delivered. Please refund." }
            };
        }

        public class FeedbackItem
        {
            public int FeedbackID { get; set; }
            public int OrderID { get; set; }
            public string CustomerName { get; set; }
            public int Rating { get; set; } // 1-5
            public string Comment { get; set; }
        }
    }
}
