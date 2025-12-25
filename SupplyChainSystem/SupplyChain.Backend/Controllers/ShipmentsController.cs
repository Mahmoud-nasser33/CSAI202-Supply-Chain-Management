// Manages HTTP requests and API logic for Shipments.
using Microsoft.AspNetCore.Mvc;
using SupplyChain.Backend.Models;
using SupplyChain.Backend.Repositories;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentsController(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetShipments()
        {
            try
            {
                var shipments = await _shipmentRepository.GetAllShipmentsAsync();
                return Ok(shipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving shipments", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipment(int id)
        {
            try
            {
                var s = await _shipmentRepository.GetShipmentByIdAsync(id);
                if (s == null) return NotFound();
                return Ok(s);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving shipment", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment([FromBody] Shipment shipment)
        {
            try
            {
                var created = await _shipmentRepository.CreateShipmentAsync(shipment);
                return CreatedAtAction(nameof(GetShipment), new { id = created.ShipmentID }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating shipment", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShipment(int id, [FromBody] Shipment shipment)
        {
            try
            {
                bool updated = await _shipmentRepository.UpdateShipmentAsync(id, shipment);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating shipment", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            try
            {
                bool deleted = await _shipmentRepository.DeleteShipmentAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting shipment", error = ex.Message });
            }
        }
    }

    public class ShipmentDto
    {
        public int Id { get; set; }
        public string? OriginWarehouse { get; set; }
        public string? DestinationAddress { get; set; }
        public string? Status { get; set; }
        public DateTime EstimatedArrival { get; set; }
        public List<ShipmentItemDto>? Items { get; set; }
    }

    public class ShipmentItemDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
