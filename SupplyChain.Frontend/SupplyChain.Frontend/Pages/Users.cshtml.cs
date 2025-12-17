using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class UsersModel : PageModel
    {
        public List<UserItem> UserList { get; set; } = new();

        public Microsoft.AspNetCore.Mvc.IActionResult OnPostDelete(int id)
        {
            // MOCK BEHAVIOR: Simulate Deletion
            return RedirectToPage();
        }

        public void OnGet()
        {
            UserList = new List<UserItem>
            {
                new UserItem { UserID = 1, Username = "admin", Email = "admin@system.com", Role = "Administrator" },
                new UserItem { UserID = 2, Username = "manager", Email = "manager@system.com", Role = "Manager" },
                new UserItem { UserID = 3, Username = "staff1", Email = "staff1@system.com", Role = "Staff" }
            };
        }

        public class UserItem
        {
            public int UserID { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }
    }
}
