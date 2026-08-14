using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages
{
    public class SuppliersModel : PageModel
    {
        private readonly SupplierService _supplierService;

        [BindProperty]
        public SupplierInput SupplierInput { get; set; } = new();

        public List<Supplier> Suppliers { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public SuppliersModel(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public void OnGet()
        {
            LoadSuppliers();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadSuppliers();
                return Page();
            }

            var result = _supplierService.Create(SupplierInput);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                LoadSuppliers();
                return Page();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var result = _supplierService.Delete(id);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                LoadSuppliers();
                return Page();
            }

            return RedirectToPage();
        }

        private void LoadSuppliers()
        {
            var result = _supplierService.GetAll();
            if (result.Success)
            {
                Suppliers = result.Data;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
    }
}
