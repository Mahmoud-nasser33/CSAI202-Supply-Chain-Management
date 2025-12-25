// Defines the CreateSupplier.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupplyChain.Backend.Models;
using System.Net.Http.Json;

namespace SupplyChain.Frontend.Pages
{
    public class CreateSupplierModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateSupplierModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Supplier Supplier { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Suppliers", Supplier);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Suppliers");
                }

                ModelState.AddModelError(string.Empty, "Error creating supplier on the server.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Connection error: {ex.Message}");
            }

            return Page();
        }
    }
}
