using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using SupplyChain.Frontend.Pages; 

namespace SupplyChain.Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

       
        public int ProductCount { get; set; } = 0;
        public int OrderCount { get; set; } = 0;
        public decimal InventoryValue { get; set; } = 0;

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");

            try
            {
             
                var products = await client.GetFromJsonAsync<List<ProductDto>>("api/Products");
                if (products != null)
                {
                    ProductCount = products.Count;
                    InventoryValue = products.Sum(p => p.Price * p.StockQuantity);
                }

                
                var orders = await client.GetFromJsonAsync<List<dynamic>>("api/Orders");
                if (orders != null)
                {
                    OrderCount = orders.Count;
                }
            }
            catch (Exception)
            {
               
            }
        }
    }
}