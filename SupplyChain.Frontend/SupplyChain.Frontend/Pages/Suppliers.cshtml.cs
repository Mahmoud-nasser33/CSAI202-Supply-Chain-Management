using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SupplyChain.Frontend.Pages
{
    public class SuppliersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<SupplierDto> Suppliers { get; set; } = new List<SupplierDto>();

        public SuppliersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
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
            var client = _httpClientFactory.CreateClient("BackendApi");
            await client.DeleteAsync($"api/Suppliers/{id}");
            return RedirectToPage();
        }
    }

    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public string Address { get; set; }
        public int LeadTimeDays { get; set; }
        public double Rating { get; set; }
    }
}
