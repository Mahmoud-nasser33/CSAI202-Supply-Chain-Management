using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace SupplyChain.Frontend.Pages
{
    public class CreateProductModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateProductModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        
        [BindProperty]
        public ProductInputDto Product { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient("BackendApi");

            
            var response = await client.PostAsJsonAsync("api/Products", Product);

            if (response.IsSuccessStatusCode)
            {
                
                return RedirectToPage("/Products");
            }
            else
            {
               
                ModelState.AddModelError(string.Empty, "Error");
                return Page();
            }
        }
    }

    public class ProductInputDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}