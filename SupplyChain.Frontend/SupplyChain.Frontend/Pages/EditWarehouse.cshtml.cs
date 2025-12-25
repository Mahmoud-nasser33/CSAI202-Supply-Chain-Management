// Defines the EditWarehouse.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class EditWarehouseModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditWarehouseModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Warehouse Warehouse { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            var warehouse = await client.GetFromJsonAsync<Warehouse>($"api/Warehouses/{id}");

            if (warehouse == null)
            {
                return RedirectToPage("/Warehouses");
            }

            Warehouse = warehouse;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Warehouses/{Warehouse.WarehouseID}", Warehouse);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Warehouses");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error updating warehouse.");
                return Page();
            }
        }
    }
}
