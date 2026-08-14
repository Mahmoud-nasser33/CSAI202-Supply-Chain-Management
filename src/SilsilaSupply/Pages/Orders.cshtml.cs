using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;

        [BindProperty]
        public OrderInput OrderInput { get; set; } = new();

        public List<Order> Orders { get; set; } = new();
        public List<SelectListItem> CustomerOptions { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public OrdersModel(OrderService orderService, CustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
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

            var result = _orderService.Create(OrderInput);
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
            var result = _orderService.Delete(id);
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
            var ordersResult = _orderService.GetAll();
            if (ordersResult.Success)
            {
                Orders = ordersResult.Data;
            }
            else
            {
                ErrorMessage = ordersResult.ErrorMessage;
            }

            var customersResult = _customerService.GetAll();
            if (customersResult.Success)
            {
                CustomerOptions = customersResult.Data
                    .Select(c => new SelectListItem($"{c.Name} — {c.Email}", c.CustomerId.ToString()))
                    .ToList();
            }
            else if (ErrorMessage == null)
            {
                ErrorMessage = customersResult.ErrorMessage;
            }
        }
    }
}
