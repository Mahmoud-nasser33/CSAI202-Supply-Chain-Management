// Defines the EditProduct.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using SupplyChain.Backend.Models;

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

        public List<CategoryDto> Categories { get; set; } = new();
        public List<SupplierDto> Suppliers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadDataAsync();
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            ProductDto product = await client.GetFromJsonAsync<ProductDto>($"api/Products/{id}");

            if (product == null)
            {
                return RedirectToPage("/Products");
            }

            Product = product;
            return Page();
        }

        private async Task LoadDataAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            try
            {
                Categories = await client.GetFromJsonAsync<List<CategoryDto>>("api/Categories") ?? new();
                Suppliers = await client.GetFromJsonAsync<List<SupplierDto>>("api/Suppliers") ?? new();
            }
            catch {  }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }

            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Products/{Product.Id}", Product);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Products");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error updating product.");
                await LoadDataAsync();
                return Page();
            }
        }
    }

}