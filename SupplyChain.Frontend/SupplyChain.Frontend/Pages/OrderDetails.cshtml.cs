// Defines the OrderDetails.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class OrderDetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderDetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Order Order { get; set; }
        public List<OrderDetail> Details { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            HttpResponseMessage response = await client.GetAsync($"api/Orders/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<OrderFullResponse>();
                if (data != null)
                {
                    Order = data.Order;
                    Details = data.Details;
                    return Page();
                }
            }

            return RedirectToPage("/Orders");
        }

        private class OrderFullResponse
        {
            public Order Order { get; set; }
            public List<OrderDetail> Details { get; set; } = new();
        }
    }
}
