// Defines the EditCustomer.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class EditCustomerModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditCustomerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Customer Customer { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            var customer = await client.GetFromJsonAsync<Customer>($"api/Customers/{id}");

            if (customer == null)
            {
                return RedirectToPage("/Customers");
            }

            Customer = customer;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Customers/{Customer.CustomerID}", Customer);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Customers");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error updating customer.");
                return Page();
            }
        }
    }
}
