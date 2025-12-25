// Defines the CreateProduct.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using SupplyChain.Backend.Models;

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
        public ProductInputDto Product { get; set; } = new();

        public List<CategoryDto> Categories { get; set; } = new();
        public List<SupplierDto> Suppliers { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
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

            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");

                var backendDto = new {
                    Name = Product.Name,
                    Price = Product.Price,
                    Description = Product.Description,
                    CategoryID = Product.CategoryID,
                    SupplierID = Product.SupplierID,
                    StockQuantity = Product.StockQuantity
                };

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Products", backendDto);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Products");
                }

                ModelState.AddModelError(string.Empty, "Error creating product on the server.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Connection error: {ex.Message}");
            }

            await LoadDataAsync();
            return Page();
        }
    }

}