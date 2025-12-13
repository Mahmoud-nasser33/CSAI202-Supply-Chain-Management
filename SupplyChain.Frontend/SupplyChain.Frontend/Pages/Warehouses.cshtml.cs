using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SupplyChain.Frontend.Pages
{
    public class WarehousesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<WarehouseDto> Warehouses { get; set; } = new List<WarehouseDto>();

        public WarehousesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            try
            {
                var result = await client.GetFromJsonAsync<List<WarehouseDto>>("api/Warehouses");
                if (result != null)
                {
                    Warehouses = result;
                }
            }
            catch
            {
                Warehouses = new List<WarehouseDto>();
            }
        }
    }

    public class WarehouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
