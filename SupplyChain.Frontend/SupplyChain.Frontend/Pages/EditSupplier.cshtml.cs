// Defines the EditSupplier.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class EditSupplierModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditSupplierModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Supplier Supplier { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            var supplier = await client.GetFromJsonAsync<Supplier>($"api/Suppliers/{id}");

            if (supplier == null)
            {
                return RedirectToPage("/Suppliers");
            }

            Supplier = supplier;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Suppliers/{Supplier.SupplierID}", Supplier);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Suppliers");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error updating supplier.");
                return Page();
            }
        }
    }
}
