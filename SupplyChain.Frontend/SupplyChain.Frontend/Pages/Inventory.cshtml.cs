// Defines the Inventory.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class InventoryModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public InventoryModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<InventoryItem> InventoryList { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                var inventory = await client.GetFromJsonAsync<List<InventoryItem>>("api/Inventory");
                if (inventory != null)
                {
                    InventoryList = inventory;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching inventory: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                HttpResponseMessage response = await client.DeleteAsync($"api/Inventory/{id}");
            }
            catch (Exception)
            {

            }
            return RedirectToPage();
        }

        public class InventoryItem
        {
            public int InventoryID { get; set; }
            public string ProductName { get; set; }
            public string WarehouseName { get; set; }
            public int Quantity_Available { get; set; }
            public int Reorder_Level { get; set; }
        }
    }
}
