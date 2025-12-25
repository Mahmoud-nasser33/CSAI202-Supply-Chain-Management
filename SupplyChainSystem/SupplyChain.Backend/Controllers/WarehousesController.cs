// Manages HTTP requests and API logic for Warehouses.
using Microsoft.AspNetCore.Mvc;
using SupplyChain.Backend.Models;
using SupplyChain.Backend.Repositories;
using System.Collections.Generic;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehousesController(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetWarehouses()
        {
            try
            {
                var warehouses = await _warehouseRepository.GetAllWarehousesAsync();
                return Ok(warehouses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving warehouses", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouse(int id)
        {
            try
            {
                var w = await _warehouseRepository.GetWarehouseByIdAsync(id);
                if (w == null) return NotFound();
                return Ok(w);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving warehouse", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] Warehouse warehouse)
        {
            try
            {
                var created = await _warehouseRepository.CreateWarehouseAsync(warehouse);
                return CreatedAtAction(nameof(GetWarehouse), new { id = created.WarehouseID }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating warehouse", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouse(int id, [FromBody] Warehouse warehouse)
        {
            try
            {
                bool updated = await _warehouseRepository.UpdateWarehouseAsync(id, warehouse);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating warehouse", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            try
            {
                bool deleted = await _warehouseRepository.DeleteWarehouseAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting warehouse", error = ex.Message });
            }
        }
    }
}
