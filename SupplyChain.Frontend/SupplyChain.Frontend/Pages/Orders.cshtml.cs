// Defines the Orders.cshtml class/logic for the Supply Chain system.
#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
        public string SearchString { get; set; }

        public OrdersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");

                HttpResponseMessage response = await client.DeleteAsync($"api/Orders/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage();
                }
            }
            catch (Exception)
            {

            }
            return RedirectToPage();
        }

        public async Task OnGet(string searchString)
        {
            SearchString = searchString;

            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                HttpResponseMessage response = await client.GetAsync("api/Orders");

                if (response.IsSuccessStatusCode)
                {
                    var apiOrders = await response.Content.ReadFromJsonAsync<List<ApiOrderDto>>();

                    if (apiOrders != null && apiOrders.Count > 0)
                    {
                        var allOrders = apiOrders.Select(o => new OrderDto
                        {
                            OrderID = o.OrderID,
                            OrderDate = o.OrderDate,
                            Status = o.Status,
                            TotalAmount = o.TotalAmount ?? 0,
                            CustomerName = o.CustomerName ?? "Unknown"
                        }).ToList();

                        if (!string.IsNullOrEmpty(searchString))
                        {
                            Orders = allOrders.Where(o =>
                                o.OrderID.ToString().Contains(searchString) ||
                                o.Status.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                (o.CustomerName != null && o.CustomerName.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList();
                        }
                        else
                        {
                            Orders = allOrders;
                        }
                    }
                    else
                    {
                        Orders = new List<OrderDto>();
                    }
                }
                else
                {

                    Orders = new List<OrderDto>();
                }
            }
            catch (Exception ex)
            {

                Orders = new List<OrderDto>();
            }
        }

        private class ApiOrderDto
        {
            public int OrderID { get; set; }
            public int CustomerID { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; } = string.Empty;
            public decimal? TotalAmount { get; set; }
            public string? CustomerName { get; set; }
        }
    }

    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }
}
