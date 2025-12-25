// Defines the CreateShipment.cshtml class/logic for the Supply Chain system.
#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class CreateShipmentModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateShipmentModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ShipmentDto Shipment { get; set; } = new();

        public void OnGet()
        {

            Shipment.Shipment_Date = DateTime.Now.AddDays(1);
            Shipment.Status = "Preparing";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Shipments", Shipment);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Shipments");
                }

                ModelState.AddModelError(string.Empty, "Error creating shipment on server.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Connection error: {ex.Message}");
            }

            return Page();
        }
    }
}
