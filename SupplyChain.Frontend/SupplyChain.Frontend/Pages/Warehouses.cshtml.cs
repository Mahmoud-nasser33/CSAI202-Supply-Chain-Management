using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SupplyChain.Frontend.Pages
{
    public class WarehousesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<WarehouseDto> Warehouses { get; set; } = new List<WarehouseDto>();

        public IActionResult OnPostDelete(int id)
        {
            // MOCK BEHAVIOR: Simulate Deletion
            return RedirectToPage();
        }

        public void OnGet()
        {
            // Rich Mock Data for Detailed View
            Warehouses = new List<WarehouseDto>
            {
                new WarehouseDto { Id = 1, Name = "Central Hub", Location = "Cairo, Egypt", Manager = "Ahmed Hassan", Utilization = 85, IsActive = true },
                new WarehouseDto { Id = 2, Name = "Port Said Depot", Location = "Port Said, Egypt", Manager = "Sarah Salem", Utilization = 45, IsActive = true },
                new WarehouseDto { Id = 3, Name = "Alexandria Cold Storage", Location = "Alexandria, Egypt", Manager = "Mohamed Ali", Utilization = 92, IsActive = true },
                new WarehouseDto { Id = 4, Name = "Giza Distribution", Location = "Giza, Egypt", Manager = "Khaled Omar", Utilization = 12, IsActive = false }
            };
        }
    }

    public class WarehouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Manager { get; set; }
        public int Utilization { get; set; }
        public bool IsActive { get; set; }
    }
}
