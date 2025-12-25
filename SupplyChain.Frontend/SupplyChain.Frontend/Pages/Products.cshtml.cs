// Defines the Products.cshtml class/logic for the Supply Chain system.
#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using System;
using SupplyChain.Backend.Models;

namespace SupplyChain.Frontend.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public int CategoryCount => Products.Select(p => p.Category).Distinct().Count();
        public int SupplierCount => Products.Select(p => p.Supplier).Distinct().Count();

        public ProductsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                HttpResponseMessage response = await client.GetAsync("api/Products");

                if (response.IsSuccessStatusCode)
                {
                    List<ApiProductDto> apiProducts = await response.Content.ReadFromJsonAsync<List<ApiProductDto>>();

                    if (apiProducts != null && apiProducts.Count > 0)
                    {

                        Products = apiProducts.Select(p => new ProductDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            SKU = $"PRD-{p.Id:D4}",
                            Category = p.CategoryName ?? "Uncategorized",
                            Price = p.Price,
                            StockQuantity = p.StockQuantity,
                            Supplier = p.SupplierName ?? "N/A",
                            Description = p.Description ?? string.Empty
                        }).ToList();
                    }
                    else
                    {
                        Products = new List<ProductDto>();
                    }
                }
                else
                {

                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"API call failed: {response.StatusCode} - {errorContent}");
                    Products = new List<ProductDto>();
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error fetching products: {ex.Message}");
                Products = new List<ProductDto>();
            }
        }

        private class ApiProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Description { get; set; } = string.Empty;
            public int CategoryID { get; set; }
            public string? CategoryName { get; set; }
            public int StockQuantity { get; set; }
            public string? SupplierName { get; set; }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("BackendApi");

            HttpResponseMessage response = await client.DeleteAsync($"api/Products/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }
            else
            {
                return Page();
            }
        }
    }
}