using Microsoft.AspNetCore.Mvc.RazorPages;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DashboardService _dashboardService;

        public DashboardStats Stats { get; set; } = new();
        public List<Order> RecentOrders { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public IndexModel(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public void OnGet()
        {
            var result = _dashboardService.GetStats();
            if (result.Success)
            {
                Stats = result.Data;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }

            var recent = _dashboardService.GetRecentOrders();
            if (recent.Success)
            {
                RecentOrders = recent.Data;
            }
        }
    }
}
