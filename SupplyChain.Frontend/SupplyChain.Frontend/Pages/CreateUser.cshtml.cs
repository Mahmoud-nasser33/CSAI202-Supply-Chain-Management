// Defines the CreateUser.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace SupplyChain.Frontend.Pages
{
    public class CreateUserModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await Task.Delay(500);
            return RedirectToPage("/Users");
        }
    }
}
