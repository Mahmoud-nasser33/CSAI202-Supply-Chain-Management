using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace SupplyChain.Frontend.Pages
{
    public class CreateWarehouseModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // MOCK BEHAVIOR: Simulate Success
            await Task.Delay(500);
            return RedirectToPage("/Warehouses");
        }
    }
}
