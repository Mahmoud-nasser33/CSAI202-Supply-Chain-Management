using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages
{
    public class InventoryModel : PageModel
    {
        private readonly InventoryService _inventoryService;
        private readonly ProductService _productService;
        private readonly WarehouseService _warehouseService;

        [BindProperty]
        public InventoryInput InventoryInput { get; set; } = new();

        public List<InventoryItem> Inventory { get; set; } = new();
        public List<SelectListItem> ProductOptions { get; set; } = new();
        public List<SelectListItem> WarehouseOptions { get; set; } = new();
        public bool LowFilterActive { get; set; }
        public string? ErrorMessage { get; set; }

        public InventoryModel(InventoryService inventoryService, ProductService productService, WarehouseService warehouseService)
        {
            _inventoryService = inventoryService;
            _productService = productService;
            _warehouseService = warehouseService;
        }

        public void OnGet(int? low)
        {
            LowFilterActive = low == 1;
            Load();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Load();
                return Page();
            }

            var result = _inventoryService.Create(InventoryInput);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                Load();
                return Page();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var result = _inventoryService.Delete(id);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                Load();
                return Page();
            }

            return RedirectToPage();
        }

        private void Load()
        {
            var inventoryResult = _inventoryService.GetAll(LowFilterActive);
            if (inventoryResult.Success)
            {
                Inventory = inventoryResult.Data;
            }
            else
            {
                ErrorMessage = inventoryResult.ErrorMessage;
            }

            var productsResult = _productService.GetAll();
            if (productsResult.Success)
            {
                ProductOptions = productsResult.Data
                    .Select(p => new SelectListItem(p.Name, p.ProductId.ToString()))
                    .ToList();
            }
            else if (ErrorMessage == null)
            {
                ErrorMessage = productsResult.ErrorMessage;
            }

            var warehousesResult = _warehouseService.GetAll();
            if (warehousesResult.Success)
            {
                WarehouseOptions = warehousesResult.Data
                    .Select(w => new SelectListItem(w.Name, w.WarehouseId.ToString()))
                    .ToList();
            }
            else if (ErrorMessage == null)
            {
                ErrorMessage = warehousesResult.ErrorMessage;
            }
        }
    }
}
