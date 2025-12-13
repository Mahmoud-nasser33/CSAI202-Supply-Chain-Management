using Microsoft.AspNetCore.Mvc;
using SupplyChain.Backend.Models;
using System.Collections.Generic;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static List<ProductDto> _products = new List<ProductDto>
        {
            new ProductDto { Id = 1, Name = "Laptop", Price = 1200, StockQuantity = 50, Description = "High performance laptop" },
            new ProductDto { Id = 2, Name = "Smartphone", Price = 800, StockQuantity = 150, Description = "Latest model smartphone" },
            new ProductDto { Id = 3, Name = "Headphones", Price = 150, StockQuantity = 8, Description = "Noise cancelling headphones" },
            new ProductDto { Id = 4, Name = "Monitor", Price = 300, StockQuantity = 20, Description = "4K Ultra HD Monitor" },
            new ProductDto { Id = 5, Name = "Keyboard", Price = 100, StockQuantity = 0, Description = "Mechanical keyboard" }
        };

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDto product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.Description = product.Description;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _products.Remove(product);
            return NoContent();
        }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }
    }
}
