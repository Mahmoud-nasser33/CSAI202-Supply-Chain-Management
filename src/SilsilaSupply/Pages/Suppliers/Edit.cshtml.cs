using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages.Suppliers
{
    public class EditModel : PageModel
    {
        private readonly SupplierService _supplierService;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public SupplierInput SupplierInput { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public EditModel(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public void OnGet()
        {
            var result = _supplierService.GetById(Id);
            if (result.Success)
            {
                SupplierInput.Name = result.Data.Name;
                SupplierInput.Email = result.Data.Email;
                SupplierInput.Rating = result.Data.Rating;
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

            var result = _supplierService.Update(Id, SupplierInput);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                return Page();
            }

            return RedirectToPage("/Suppliers");
        }
    }
}
