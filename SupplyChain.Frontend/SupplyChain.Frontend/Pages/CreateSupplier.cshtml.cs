using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SupplyChain.Frontend.Pages
{
    public class CreateSupplierModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public SupplierDto Supplier { get; set; } = new SupplierDto();

        public CreateSupplierModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient("BackendApi");
            var response = await client.PostAsJsonAsync("api/Suppliers", Supplier);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Suppliers");
            }

            ModelState.AddModelError(string.Empty, "Error creating supplier.");
            return Page();
        }
    }
}
