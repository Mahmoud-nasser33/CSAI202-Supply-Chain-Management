using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SilsilaSupply.Models;
using SilsilaSupply.Services;

namespace SilsilaSupply.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public OrderInput OrderInput { get; set; } = new();

        public List<SelectListItem> CustomerOptions { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public EditModel(OrderService orderService, CustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        public void OnGet()
        {
            LoadCustomers();
            var result = _orderService.GetById(Id);
            if (result.Success)
            {
                OrderInput.CustomerId = result.Data.CustomerId ?? 0;
                OrderInput.TotalAmount = result.Data.TotalAmount ?? 0;
                OrderInput.Status = result.Data.Status;
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
                LoadCustomers();
                return Page();
            }

            var result = _orderService.Update(Id, OrderInput);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                LoadCustomers();
                return Page();
            }

            return RedirectToPage("/Orders");
        }

        private void LoadCustomers()
        {
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
