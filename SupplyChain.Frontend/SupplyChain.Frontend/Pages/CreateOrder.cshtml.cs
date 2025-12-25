// Defines the CreateOrder.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class CreateOrderModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateOrderModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public SelectList ProductOptions { get; set; }
        public SelectList CustomerOptions { get; set; }

        [BindProperty]
        public OrderInputDto Order { get; set; } = new OrderInputDto();

        public async Task OnGetAsync()
        {
            await LoadOptionsAsync();
        }

        private async Task LoadOptionsAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");
            try
            {
                List<ProductOptionDto> products = await client.GetFromJsonAsync<List<ProductOptionDto>>("api/Products") ?? new();
                ProductOptions = new SelectList(products, "Id", "Name");

                List<CustomerOptionDto> customers = await client.GetFromJsonAsync<List<CustomerOptionDto>>("api/Customers") ?? new();
                CustomerOptions = new SelectList(customers, "CustomerID", "Name");
            }
            catch (Exception)
            {
                ProductOptions = new SelectList(new List<ProductOptionDto>(), "Id", "Name");
                CustomerOptions = new SelectList(new List<CustomerOptionDto>(), "CustomerID", "Name");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadOptionsAsync();
                return Page();
            }

            HttpClient client = _httpClientFactory.CreateClient("BackendApi");

            try
            {

                ProductOptionDto product = await client.GetFromJsonAsync<ProductOptionDto>($"api/Products/{Order.ProductId}");
                if (product == null)
                {
                    ModelState.AddModelError(string.Empty, "Selected product not found.");
                    await LoadOptionsAsync();
                    return Page();
                }

                var request = new {
                    CustomerID = Order.CustomerId,
                    OrderDetails = new[] {
                        new {
                            ProductID = Order.ProductId,
                            Quantity = Order.Quantity,
                            UnitPrice = product.Price
                        }
                    }
                };

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Orders", request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Orders");
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Error creating order: {errorMsg}");
            }
            catch (Exception ex)
            {
                 ModelState.AddModelError(string.Empty, $"Communication error: {ex.Message}");
            }

            await LoadOptionsAsync();
            return Page();
        }

        public class ProductOptionDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
        }

        public class CustomerOptionDto
        {
            public int CustomerID { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class OrderInputDto
        {
            public int CustomerId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}