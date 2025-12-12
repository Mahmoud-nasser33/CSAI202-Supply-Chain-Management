using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace SupplyChain.Frontend.Pages
{
    public class EditProductModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditProductModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        
        public ProductDto Product { get; set; } = new ProductDto();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            var product = await client.GetFromJsonAsync<ProductDto>($"api/Products/{id}");

            if (product == null)
            {
                return RedirectToPage("/Products");
            }

            Product = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient("BackendApi");
            var response = await client.PutAsJsonAsync($"api/Products/{Product.Id}", Product);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Products");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error updating product.");
                return Page();
            }
        }
    }

    
}