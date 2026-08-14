using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages
{
    public class ShipmentsModel : PageModel
    {
        private readonly ShipmentService _shipmentService;
        private readonly OrderService _orderService;
        private readonly WarehouseService _warehouseService;

        [BindProperty]
        public ShipmentInput ShipmentInput { get; set; } = new();

        public List<Shipment> Shipments { get; set; } = new();
        public List<SelectListItem> OrderOptions { get; set; } = new();
        public List<SelectListItem> WarehouseOptions { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public ShipmentsModel(ShipmentService shipmentService, OrderService orderService, WarehouseService warehouseService)
        {
            _shipmentService = shipmentService;
            _orderService = orderService;
            _warehouseService = warehouseService;
        }

        public void OnGet()
        {
            Load();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Load();
                return Page();
            }

            var result = _shipmentService.Create(ShipmentInput);
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
            var result = _shipmentService.Delete(id);
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
            var shipmentsResult = _shipmentService.GetAll();
            if (shipmentsResult.Success)
            {
                Shipments = shipmentsResult.Data;
            }
            else
            {
                ErrorMessage = shipmentsResult.ErrorMessage;
            }

            var ordersResult = _orderService.GetAll();
            if (ordersResult.Success)
            {
                OrderOptions = ordersResult.Data
                    .Select(o => new SelectListItem($"#{o.OrderId} — {o.CustomerName ?? "Unknown customer"}", o.OrderId.ToString()))
                    .ToList();
            }
            else if (ErrorMessage == null)
            {
                ErrorMessage = ordersResult.ErrorMessage;
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
