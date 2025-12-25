// Defines the EditInventory.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class EditInventoryModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditInventoryModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Inventory Inventory { get; set; } = new();

        public SelectList ProductOptions { get; set; }
        public SelectList WarehouseOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            var item = await client.GetFromJsonAsync<Inventory>($"api/Inventory/{id}");

            if (item == null)
            {
                return RedirectToPage("/Inventory");
            }

            Inventory = item;
            await LoadOptionsAsync();
            return Page();
        }

        private async Task LoadOptionsAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            try
            {
                List<ProductOption> products = await client.GetFromJsonAsync<List<ProductOption>>("api/Products") ?? new();
                ProductOptions = new SelectList(products, "Id", "Name");

                List<WarehouseOption> warehouses = await client.GetFromJsonAsync<List<WarehouseOption>>("api/Warehouses") ?? new();
                WarehouseOptions = new SelectList(warehouses, "WarehouseID", "Name");
            }
            catch (Exception)
            {
                ProductOptions = new SelectList(new List<ProductOption>(), "Id", "Name");
                WarehouseOptions = new SelectList(new List<WarehouseOption>(), "WarehouseID", "Name");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadOptionsAsync();
                return Page();
            }

            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Inventory/{Inventory.InventoryID}", Inventory);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Inventory");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error updating inventory record.");
                await LoadOptionsAsync();
                return Page();
            }
        }

        private class ProductOption { public int Id { get; set; } public string Name { get; set; } = string.Empty; }
        private class WarehouseOption { public int WarehouseID { get; set; } public string Name { get; set; } = string.Empty; }
    }
}
