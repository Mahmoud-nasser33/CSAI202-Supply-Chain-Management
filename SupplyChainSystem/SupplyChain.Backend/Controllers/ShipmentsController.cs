using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private static List<ShipmentDto> _shipments = new List<ShipmentDto>
        {
            new ShipmentDto 
            { 
                Id = 1, 
                OriginWarehouse = "Central Hub", 
                DestinationAddress = "123 Main St, Springfield", 
                Status = "In Transit",
                EstimatedArrival = DateTime.Now.AddDays(2),
                Items = new List<ShipmentItemDto>
                {
                    new ShipmentItemDto { ProductName = "Laptop", Quantity = 1 },
                    new ShipmentItemDto { ProductName = "Mouse", Quantity = 2 }
                }
            },
            new ShipmentDto 
            { 
                Id = 2, 
                OriginWarehouse = "West Coast Distribution", 
                DestinationAddress = "456 Elm St, Shelbyville", 
                Status = "Delivered",
                EstimatedArrival = DateTime.Now.AddDays(-1),
                Items = new List<ShipmentItemDto>
                {
                    new ShipmentItemDto { ProductName = "Monitor", Quantity = 10 }
                }
            }
        };

        [HttpGet]
        public IActionResult GetShipments()
        {
            return Ok(_shipments);
        }
    }

    public class ShipmentDto
    {
        public int Id { get; set; }
        public string OriginWarehouse { get; set; }
        public string DestinationAddress { get; set; }
        public string Status { get; set; }
        public DateTime EstimatedArrival { get; set; }
        public List<ShipmentItemDto> Items { get; set; }
    }

    public class ShipmentItemDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
