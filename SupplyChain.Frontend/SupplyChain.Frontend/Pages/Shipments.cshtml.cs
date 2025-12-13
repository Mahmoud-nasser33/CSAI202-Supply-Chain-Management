using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SupplyChain.Frontend.Pages
{
    public class ShipmentsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<ShipmentDto> Shipments { get; set; } = new List<ShipmentDto>();

        public ShipmentsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            try
            {
                var result = await client.GetFromJsonAsync<List<ShipmentDto>>("api/Shipments");
                if (result != null)
                {
                    Shipments = result;
                }
            }
            catch
            {
                Shipments = new List<ShipmentDto>();
            }
        }
    }

    public class ShipmentDto
    {
        public int Id { get; set; }
        public string OriginWarehouse { get; set; }
        public string DestinationAddress { get; set; }
        public string Status { get; set; }
        public DateTime EstimatedArrival { get; set; }
        public List<ShipmentItemDto> Items { get; set; }
    }

    public class ShipmentItemDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
