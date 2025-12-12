using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace SupplyChain.Frontend.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        public ProductsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            try
            {
                var result = await client.GetFromJsonAsync<List<ProductDto>>("api/Products");
                if (result != null)
                {
                    Products = result;
                }
            }
            catch (Exception)
            {
                Products = new List<ProductDto>();
            }
        } 

        
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");

            var response = await client.DeleteAsync($"api/Products/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }
            else
            {
                return Page();
            }
        }
    } 
} 