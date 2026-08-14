using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly ProductService _productService;

        [BindProperty]
        public ProductInput ProductInput { get; set; } = new();

        public List<Product> Products { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public ProductsModel(ProductService productService)
        {
            _productService = productService;
        }

        public void OnGet()
        {
            LoadProducts();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadProducts();
                return Page();
            }

            var result = _productService.Create(ProductInput);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                LoadProducts();
                return Page();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var result = _productService.Delete(id);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                LoadProducts();
                return Page();
            }

            return RedirectToPage();
        }

        private void LoadProducts()
        {
            var result = _productService.GetAll();
            if (result.Success)
            {
                Products = result.Data;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
    }
}
