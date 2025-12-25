// Defines the Feedback.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class FeedbackModel : PageModel
    {
        public List<FeedbackItem> FeedbackList { get; set; } = new();

        public void OnGet()
        {
            // Empty until backend API is implemented.
            FeedbackList = new List<FeedbackItem>();
        }

        public class FeedbackItem
        {
            public int FeedbackID { get; set; }
            public int OrderID { get; set; }
            public string CustomerName { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
        }
    }
}
