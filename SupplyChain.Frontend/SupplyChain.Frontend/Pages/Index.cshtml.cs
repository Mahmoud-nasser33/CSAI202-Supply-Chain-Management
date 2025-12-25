// Defines the Index.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using SupplyChain.Frontend.Pages;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public DashboardSummaryDto Summary { get; set; } = new();

        public async Task OnGetAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");

            try
            {
                var summary = await client.GetFromJsonAsync<DashboardSummaryDto>("api/DashboardSummary");
                if (summary != null)
                {
                    Summary = summary;
                }
            }
            catch (Exception)
            {

            }
        }

        public class DashboardSummaryDto
        {
            public int ProductCount { get; set; }
            public int OrderCount { get; set; }
            public decimal TotalRevenue { get; set; }
            public int LowStockCount { get; set; }
            public List<ActivityItemDto> RecentActivity { get; set; } = new();
        }

        public class ActivityItemDto
        {
            public string Type { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public DateTime Date { get; set; }
        }
    }
}