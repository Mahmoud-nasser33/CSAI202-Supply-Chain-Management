using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace SupplyChain.Frontend.Pages
{
    public class CreateInventoryModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // MOCK BEHAVIOR: Simulate Success
            await Task.Delay(500);
            
            // Redirect back to Inventory
            return RedirectToPage("/Inventory");
        }
    }
}
