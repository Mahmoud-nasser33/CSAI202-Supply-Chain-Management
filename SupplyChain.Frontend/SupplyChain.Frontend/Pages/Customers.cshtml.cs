// Defines the Customers.cshtml class/logic for the Supply Chain system.
#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class CustomersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<CustomerItem> CustomerList { get; set; } = new();
        public int TotalRegistered => CustomerList.Count;
        public int ActiveThisMonth => (int)(CustomerList.Count * 0.77);
        public string RetentionRate => "88%";

        public async Task OnGetAsync()
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                List<CustomerItem> customers = await client.GetFromJsonAsync<List<CustomerItem>>("api/Customers");
                if (customers != null)
                {
                    CustomerList = customers;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching customers: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                HttpResponseMessage response = await client.DeleteAsync($"api/Customers/{id}");
                if (!response.IsSuccessStatusCode)
                {

                }
            }
            catch (Exception)
            {

            }
            return RedirectToPage();
        }

        public class CustomerItem
        {
            public int CustomerID { get; set; }
            public string Name { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? Address { get; set; }
            public string? City { get; set; }
            public string? Country { get; set; }
            public int TotalOrders { get; set; }
        }
    }
}
