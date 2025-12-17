using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace SupplyChain.Frontend.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        public ProductsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            // Mock Data for "Amazing" & "Logical" Experience
            Products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Industrial Steel Sheets", SKU = "STL-001", Category = "Raw Materials", Price = 1200.50m, StockQuantity = 450, Supplier = "MetalWorks Co." },
                new ProductDto { Id = 2, Name = "Copper Wire Spools", SKU = "CPR-009", Category = "Electronics", Price = 85.00m, StockQuantity = 12, Supplier = "ElectroGlobal" },
                new ProductDto { Id = 3, Name = "Hydraulic Pump X200", SKU = "PMP-200", Category = "Machinery", Price = 2500.00m, StockQuantity = 5, Supplier = "HeavyMech Inc." },
                new ProductDto { Id = 4, Name = "Circuit Board V5", SKU = "PCB-005", Category = "Electronics", Price = 45.99m, StockQuantity = 1200, Supplier = "TechSource" },
                new ProductDto { Id = 5, Name = "Safety Helmets (Yellow)", SKU = "SAF-099", Category = "Safety Gear", Price = 15.00m, StockQuantity = 300, Supplier = "SafeGuard" },
                new ProductDto { Id = 6, Name = "Conveyor Belt (5m)", SKU = "CNV-005", Category = "Logistics", Price = 350.00m, StockQuantity = 8, Supplier = "HeavyMech Inc." }
            };
            
            await Task.CompletedTask;
        } 

        
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");

            var response = await client.DeleteAsync($"api/Products/{id}");

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