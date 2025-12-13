using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private static List<SupplierDto> _suppliers = new List<SupplierDto>
        {
            new SupplierDto { Id = 1, Name = "TechParts Inc.", ContactInfo = "contact@techparts.com", Address = "123 Silicon Valley", LeadTimeDays = 5, Rating = 4.8 },
            new SupplierDto { Id = 2, Name = "Global Shipping Co.", ContactInfo = "info@globalshipping.com", Address = "456 Logistics Ave", LeadTimeDays = 14, Rating = 4.2 },
            new SupplierDto { Id = 3, Name = "MegaCorp Supplies", ContactInfo = "sales@megacorp.com", Address = "789 Enterprise Blvd", LeadTimeDays = 3, Rating = 3.5 }
        };

        [HttpGet]
        public IActionResult GetSuppliers()
        {
            return Ok(_suppliers);
        }

        [HttpPost]
        public IActionResult CreateSupplier([FromBody] SupplierDto supplier)
        {
            supplier.Id = _suppliers.Any() ? _suppliers.Max(s => s.Id) + 1 : 1;
            _suppliers.Add(supplier);
            return CreatedAtAction(nameof(GetSuppliers), new { id = supplier.Id }, supplier);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = _suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            _suppliers.Remove(supplier);
            return NoContent();
        }
    }

    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public string Address { get; set; }
        public int LeadTimeDays { get; set; }
        public double Rating { get; set; }
    }
}
