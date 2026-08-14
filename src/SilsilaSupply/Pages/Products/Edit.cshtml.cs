using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly ProductService _productService;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public ProductInput ProductInput { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public EditModel(ProductService productService)
        {
            _productService = productService;
        }

        public void OnGet()
        {
            var result = _productService.GetById(Id);
            if (result.Success)
            {
                ProductInput.Name = result.Data.Name;
                ProductInput.Price = result.Data.Price;
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

            var result = _productService.Update(Id, ProductInput);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                return Page();
            }

            return RedirectToPage("/Products");
        }
    }
}
