using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System.Net.Http.Json;

namespace SupplyChain.Frontend.Pages
{
    public class CreateOrderModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateOrderModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

       
        public SelectList ProductOptions { get; set; }

        [BindProperty]
        public OrderInputDto Order { get; set; } = new OrderInputDto();

        public async Task OnGetAsync()
        {
           
            var client = _httpClientFactory.CreateClient("BackendApi");
            var products = await client.GetFromJsonAsync<List<ProductDto>>("api/Products");

           
            if (products != null)
            {
                ProductOptions = new SelectList(products, "Id", "Name");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
               
                await OnGetAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("BackendApi");

            
            Order.OrderDate = DateTime.Now;
            Order.Status = "Processing"; 

            
            var response = await client.PostAsJsonAsync("api/Orders", Order);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Orders");
            }

            ModelState.AddModelError(string.Empty, "Failed to create order.");
            await OnGetAsync();
            return Page();
        }
    }

   
    public class OrderInputDto
    {
        public int ProductId { get; set; } 
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}