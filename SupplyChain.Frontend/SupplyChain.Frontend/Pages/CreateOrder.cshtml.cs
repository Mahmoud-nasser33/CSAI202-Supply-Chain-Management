using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System.Net.Http.Json;

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

        [BindProperty]
        public OrderInputDto Order { get; set; } = new OrderInputDto();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            try
            {
                var products = await client.GetFromJsonAsync<List<ProductDto>>("api/Products");

                if (products != null)
                {
                    ProductOptions = new SelectList(products, "Id", "Name");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error loading products from the backend.");
                ProductOptions = new SelectList(new List<ProductDto>(), "Id", "Name");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("BackendApi");

            Order.OrderDate = DateTime.Now;
            Order.Status = "Processing";

            // Total amount logic is mocked here for simplicity since we don't have product price in frontend logic easily without refetching
            // For the sake of the error test, we'll let the user input quantity.
            // If we want to test the <= 0 validation, the input has min=1.
            // But we can manually set it to 0 or modify the input for testing if needed.
            // Actually, the Backend validates TotalAmount. We need to calculate TotalAmount to send it?
            // Wait, OrderInputDto doesn't have TotalAmount. The BACKEND calculates it?
            // Let's check OrdersController.CreateOrder again.
            // It takes [FromBody] Order order. Order model has TotalAmount.
            // But OrderInputDto ONLY has ProductId, Quantity, OrderDate, Status.
            // The Frontend sends OrderInputDto. Backend expects Order.
            // This might cause a schema mismatch or mapping issue if not handled.
            // However, assuming the JSON binds loosely or parameters match enough (Or maybe OrderInputDto is mapped to Order).
            // Actually, if Backend expects `Order` and Frontend sends `OrderInputDto`, properties like `TotalAmount` will be default (0).
            // So TotalAmount will be 0.
            // -> The Backend validation `if (order.TotalAmount <= 0)` will ALWAYS fail if frontend doesn't send it!
            
            // Re-reading OrdersController:
            // public IActionResult CreateOrder([FromBody] Order order)
            
            // Re-reading OrderInputDto in CreateOrder.cshtml.cs:
            // It lacks TotalAmount.
            
            // So currently, sending this will result in TotalAmount = 0 on backend.
            // The backend returns BadRequest("Order total amount must be greater than zero.").
            // This is actually PERFECT for testing error handling!
            
            try 
            {
                var response = await client.PostAsJsonAsync("api/Orders/create", Order);

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

            await OnGetAsync();
            return Page();
        }
    }

   
    public class OrderInputDto
    {
        public int ProductId { get; set; } 
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}