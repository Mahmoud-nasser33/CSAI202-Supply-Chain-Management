using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages.Shipments
{
    public class EditModel : PageModel
    {
        private readonly ShipmentService _shipmentService;
        private readonly WarehouseService _warehouseService;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public ShipmentInput ShipmentInput { get; set; } = new();

        public List<SelectListItem> WarehouseOptions { get; set; } = new();
        public string? OrderReference { get; set; }
        public string? CustomerName { get; set; }
        public string? ErrorMessage { get; set; }

        public EditModel(ShipmentService shipmentService, WarehouseService warehouseService)
        {
            _shipmentService = shipmentService;
            _warehouseService = warehouseService;
        }

        public void OnGet()
        {
            LoadWarehouses();
            var result = _shipmentService.GetById(Id);
            if (result.Success)
            {
                ShipmentInput.OrderId = result.Data.OrderId;
                ShipmentInput.WarehouseId = result.Data.WarehouseId ?? 0;
                ShipmentInput.ShippedVia = result.Data.ShippedVia;
                ShipmentInput.Status = result.Data.Status;
                OrderReference = $"#{result.Data.OrderId}";
                CustomerName = result.Data.CustomerName;
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
                LoadWarehouses();
                return Page();
            }

            var result = _shipmentService.Update(Id, ShipmentInput);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                LoadWarehouses();
                return Page();
            }

            return RedirectToPage("/Shipments");
        }

        private void LoadWarehouses()
        {
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
