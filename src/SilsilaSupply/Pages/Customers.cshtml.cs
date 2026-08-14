using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages
{
    public class CustomersModel : PageModel
    {
        private readonly CustomerService _customerService;

        [BindProperty]
        public CustomerInput CustomerInput { get; set; } = new();

        public List<Customer> Customers { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public CustomersModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public void OnGet()
        {
            LoadCustomers();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadCustomers();
                return Page();
            }

            var result = _customerService.Create(CustomerInput);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                LoadCustomers();
                return Page();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var result = _customerService.Delete(id);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                LoadCustomers();
                return Page();
            }

            return RedirectToPage();
        }

        private void LoadCustomers()
        {
            var result = _customerService.GetAll();
            if (result.Success)
            {
                Customers = result.Data;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
    }
}
