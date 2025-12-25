// Defines the Warehouses.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class WarehousesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WarehousesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<Warehouse> Warehouses { get; set; } = new List<Warehouse>();

        public async Task OnGetAsync()
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                var warehouses = await client.GetFromJsonAsync<List<Warehouse>>("api/Warehouses");
                if (warehouses != null)
                {
                    Warehouses = warehouses;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching warehouses: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                HttpResponseMessage response = await client.DeleteAsync($"api/Warehouses/{id}");
            }
            catch (Exception)
            {

            }
            return RedirectToPage();
        }
    }
}
