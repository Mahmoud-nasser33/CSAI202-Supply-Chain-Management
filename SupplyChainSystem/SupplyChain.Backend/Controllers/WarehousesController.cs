using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private static List<WarehouseDto> _warehouses = new List<WarehouseDto>
        {
            new WarehouseDto { Id = 1, Name = "Central Hub", Location = "New York, NY" },
            new WarehouseDto { Id = 2, Name = "West Coast Distribution", Location = "Los Angeles, CA" },
            new WarehouseDto { Id = 3, Name = "European Branch", Location = "Berlin, Germany" }
        };

        [HttpGet]
        public IActionResult GetWarehouses()
        {
            return Ok(_warehouses);
        }
    }

    public class WarehouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
