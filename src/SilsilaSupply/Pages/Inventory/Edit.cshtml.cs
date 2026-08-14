using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages.Inventory
{
    public class EditModel : PageModel
    {
        private readonly InventoryService _inventoryService;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public InventoryQuantityInput QuantityInput { get; set; } = new();

        public string? ProductName { get; set; }
        public string? WarehouseName { get; set; }
        public string? ErrorMessage { get; set; }

        public EditModel(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public void OnGet()
        {
            var result = _inventoryService.GetById(Id);
            if (result.Success)
            {
                ProductName = result.Data.ProductName;
                WarehouseName = result.Data.WarehouseName;
                QuantityInput.Quantity = result.Data.Quantity;
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

            var result = _inventoryService.SetQuantity(Id, QuantityInput.Quantity);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                return Page();
            }

            return RedirectToPage("/Inventory");
        }
    }
}
