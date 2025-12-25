// Defines the Shipments.cshtml class/logic for the Supply Chain system.
#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class ShipmentsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ShipmentsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ShipmentDto> Shipments { get; set; } = new List<ShipmentDto>();

        public async Task OnGetAsync()
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                var shipments = await client.GetFromJsonAsync<List<ShipmentDto>>("api/Shipments");
                if (shipments != null)
                {
                    Shipments = shipments;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching shipments: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                HttpResponseMessage response = await client.DeleteAsync($"api/Shipments/{id}");
            }
            catch (Exception)
            {

            }
            return RedirectToPage();
        }
    }

    public class ShipmentDto
    {
        public int ShipmentID { get; set; }
        public int OrderID { get; set; }
        public int WarehouseID { get; set; }
        public string? WarehouseName { get; set; }
        public string? CustomerName { get; set; }
        public string? Status { get; set; }
        public string? Shipped_Via { get; set; }
        public DateTime? Shipment_Date { get; set; }

        public int Progress => Status == "Delivered" ? 100 : (Status == "In Transit" ? 60 : 20);
        public List<ShipmentItemDto> Items { get; set; } = new();
    }

    public class ShipmentItemDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
