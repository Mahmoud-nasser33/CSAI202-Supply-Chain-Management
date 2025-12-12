using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json; 

namespace SupplyChain.Frontend.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        
        public OrdersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();

        public async Task OnGet()
        {
            
            var client = _httpClientFactory.CreateClient("BackendApi");

            try
            {
               
                Orders = await client.GetFromJsonAsync<List<OrderDto>>("api/Orders/history/1");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error fetching orders: {ex.Message}");
            }
        }
    }

    
    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
