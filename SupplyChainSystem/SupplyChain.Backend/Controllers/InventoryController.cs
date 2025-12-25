// Manages HTTP requests and API logic for Inventory.
using Microsoft.AspNetCore.Mvc;
using SupplyChain.Backend.Models;
using SupplyChain.Backend.Repositories;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventory()
        {
            try
            {
                var inventory = await _inventoryRepository.GetAllInventoryAsync();
                return Ok(inventory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving inventory", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventory(int id)
        {
            try
            {
                var i = await _inventoryRepository.GetInventoryByIdAsync(id);
                if (i == null) return NotFound();
                return Ok(i);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving inventory item", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody] Inventory inventory)
        {
            try
            {
                var created = await _inventoryRepository.CreateInventoryAsync(inventory);
                return CreatedAtAction(nameof(GetInventory), new { id = created.InventoryID }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating inventory item", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, [FromBody] Inventory inventory)
        {
            try
            {
                bool updated = await _inventoryRepository.UpdateInventoryAsync(id, inventory);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating inventory item", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            try
            {
                bool deleted = await _inventoryRepository.DeleteInventoryAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting inventory item", error = ex.Message });
            }
        }
    }
}
