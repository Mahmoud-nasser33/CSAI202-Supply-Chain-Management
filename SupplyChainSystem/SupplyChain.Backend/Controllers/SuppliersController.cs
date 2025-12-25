// Manages HTTP requests and API logic for Suppliers.
using Microsoft.AspNetCore.Mvc;
using SupplyChain.Backend.Models;
using SupplyChain.Backend.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            try
            {
                var suppliers = await _supplierRepository.GetAllSuppliersAsync();
                var supplierDtos = suppliers.Select(s => new SupplierDto
                {
                    Id = s.SupplierID,
                    Name = s.Name,
                    ContactInfo = s.Contact_Info,
                    Email = s.Email,
                    Address = s.Address,
                    LeadTimeDays = s.LeadTimeDays,
                    Rating = (double)s.Rating
                }).ToList();
                return Ok(supplierDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving suppliers", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            try
            {
                var s = await _supplierRepository.GetSupplierByIdAsync(id);
                if (s == null) return NotFound();
                return Ok(new SupplierDto
                {
                    Id = s.SupplierID,
                    Name = s.Name,
                    ContactInfo = s.Contact_Info,
                    Email = s.Email,
                    Address = s.Address,
                    LeadTimeDays = s.LeadTimeDays,
                    Rating = (double)s.Rating
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving supplier", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] Supplier supplier)
        {
            try
            {
                var created = await _supplierRepository.CreateSupplierAsync(supplier);
                return CreatedAtAction(nameof(GetSupplier), new { id = created.SupplierID }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating supplier", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] Supplier supplier)
        {
            try
            {
                bool updated = await _supplierRepository.UpdateSupplierAsync(id, supplier);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating supplier", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                bool deleted = await _supplierRepository.DeleteSupplierAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting supplier", error = ex.Message });
            }
        }
    }

    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactInfo { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int LeadTimeDays { get; set; }
        public double Rating { get; set; }
    }
}
