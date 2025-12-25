// Defines the Suppliers.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class SuppliersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<SupplierDto> Suppliers { get; set; } = new List<SupplierDto>();
        public int TotalSuppliers => Suppliers.Count;

        public SuppliersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            try
            {
                var result = await client.GetFromJsonAsync<List<SupplierDto>>("api/Suppliers");
                if (result != null)
                {
                    Suppliers = result;
                }
            }
            catch
            {
                Suppliers = new List<SupplierDto>();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            await client.DeleteAsync($"api/Suppliers/{id}");
            return RedirectToPage();
        }
    }
}
