// Defines the Analytics.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class AnalyticsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AnalyticsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public AnalyticsDataDto Data { get; set; } = new();

        public async Task OnGetAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            try
            {
                var analytics = await client.GetFromJsonAsync<AnalyticsDataDto>("api/DashboardSummary/analytics");
                if (analytics != null)
                {
                    Data = analytics;
                }
            }
            catch (Exception)
            {

            }
        }

        public class AnalyticsDataDto
        {
            public List<string> RevenueLabels { get; set; } = new();
            public List<decimal> RevenueValues { get; set; } = new();
            public List<string> CategoryLabels { get; set; } = new();
            public List<int> CategoryValues { get; set; } = new();
            public decimal TotalRevenue => RevenueValues.Sum();
            public string RiskLevel => TotalRevenue > 100000 ? "Minimal" : "Low";
            public string EfficiencyScore => "94%";
        }
    }
}
