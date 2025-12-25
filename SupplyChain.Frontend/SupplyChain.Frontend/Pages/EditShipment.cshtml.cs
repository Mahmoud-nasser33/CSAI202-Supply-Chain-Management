// Defines the EditShipment.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class EditShipmentModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditShipmentModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Shipment Shipment { get; set; } = new();

        public SelectList StatusOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            Shipment res = await client.GetFromJsonAsync<Shipment>($"api/Shipments/{id}");

            if (res == null)
            {
                return RedirectToPage("/Shipments");
            }

            Shipment = res;
            LoadStatusOptions();
            return Page();
        }

        private void LoadStatusOptions()
        {
            List<string> list = new List<string> { "Preparing", "In Transit", "Delivered", "Cancelled" };
            StatusOptions = new SelectList(list);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadStatusOptions();
                return Page();
            }

            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Shipments/{Shipment.ShipmentID}", Shipment);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Shipments");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error updating shipment status.");
                LoadStatusOptions();
                return Page();
            }
        }
    }
}
