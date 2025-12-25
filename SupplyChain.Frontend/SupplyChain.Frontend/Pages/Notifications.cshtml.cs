// Defines the Notifications.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class NotificationsModel : PageModel
    {
        public List<NotificationItem> NotificationList { get; set; } = new();

        public void OnGet()
        {
            // Empty until backend API is implemented.
            NotificationList = new List<NotificationItem>();
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
