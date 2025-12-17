using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class NotificationsModel : PageModel
    {
        public List<NotificationItem> NotificationList { get; set; } = new();

        public void OnGet()
        {
            NotificationList = new List<NotificationItem>
            {
                new NotificationItem { ID = 1, Message = "Order #123 has been shipped.", Type = "Order Update", Date = DateTime.Now.AddHours(-2), IsRead = false },
                new NotificationItem { ID = 2, Message = "Low stock alert: Headphones (Qty: 8)", Type = "Inventory Alert", Date = DateTime.Now.AddDays(-1), IsRead = true },
                new NotificationItem { ID = 3, Message = "New supplier 'TechDistro' added.", Type = "System", Date = DateTime.Now.AddDays(-2), IsRead = true }
            };
        }

        public class NotificationItem
        {
            public int ID { get; set; }
            public string Message { get; set; }
            public string Type { get; set; }
            public DateTime Date { get; set; }
            public bool IsRead { get; set; }
        }
    }
}
