using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly CustomerService _customerService;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public CustomerInput CustomerInput { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public EditModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public void OnGet()
        {
            var result = _customerService.GetById(Id);
            if (result.Success)
            {
                CustomerInput.Name = result.Data.Name;
                CustomerInput.Email = result.Data.Email;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = _customerService.Update(Id, CustomerInput);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                return Page();
            }

            return RedirectToPage("/Customers");
        }
    }
}
