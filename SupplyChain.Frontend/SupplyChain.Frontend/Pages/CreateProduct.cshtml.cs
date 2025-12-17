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
                // In a real app, logic would go here.
                // For this demo, we proceed even if simple validation passes.
            }

            // MOCK BEHAVIOR: Simulate successful API call
            await Task.Delay(500); // Fake network delay

            // Redirect to Products page where the user expects to go
            return RedirectToPage("/Products");
        }
    }

    public class ProductInputDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}