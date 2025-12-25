// Manages HTTP requests and API logic for Products.
using Microsoft.AspNetCore.Mvc;
using SupplyChain.Backend.Models;
using SupplyChain.Backend.Repositories;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                List<Product> products = await _productRepository.GetAllProductsAsync();
                List<ProductDto> productDtos = products.Select(p => new ProductDto
                {
                    Id = p.Product_ID,
                    Name = p.Product_Name,
                    Price = p.Price,
                    Description = p.Description ?? string.Empty,
                    CategoryID = p.CategoryID,
                    CategoryName = p.CategoryName,
                    StockQuantity = p.StockQuantity,
                    SupplierID = p.SupplierID ?? 0,
                    SupplierName = p.SupplierName
                }).ToList();
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving products", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                Product product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                ProductDto productDto = new ProductDto
                {
                    Id = product.Product_ID,
                    Name = product.Product_Name,
                    Price = product.Price,
                    Description = product.Description ?? string.Empty,
                    CategoryID = product.CategoryID,
                    CategoryName = product.CategoryName,
                    StockQuantity = product.StockQuantity,
                    SupplierID = product.SupplierID ?? 0,
                    SupplierName = product.SupplierName
                };

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving product", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Product product = new Product
                {
                    Product_Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    CategoryID = productDto.CategoryID,
                    SupplierID = productDto.SupplierID > 0 ? productDto.SupplierID : null
                };

                Product createdProduct = await _productRepository.CreateProductAsync(product);
                productDto.Id = createdProduct.Product_ID;

                return CreatedAtAction(nameof(GetProduct), new { id = productDto.Id }, productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating product", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Product product = new Product
                {
                    Product_Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    CategoryID = productDto.CategoryID,
                    SupplierID = productDto.SupplierID > 0 ? productDto.SupplierID : null
                };

                bool updated = await _productRepository.UpdateProductAsync(id, product);
                if (!updated)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating product", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                bool deleted = await _productRepository.DeleteProductAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting product", error = ex.Message });
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                List<Product> products = await _productRepository.GetProductsByCategoryAsync(categoryId);
                List<ProductDto> productDtos = products.Select(p => new ProductDto
                {
                    Id = p.Product_ID,
                    Name = p.Product_Name,
                    Price = p.Price,
                    Description = p.Description ?? string.Empty,
                    CategoryID = p.CategoryID,
                    CategoryName = p.CategoryName,
                    StockQuantity = p.StockQuantity,
                    SupplierID = p.SupplierID ?? 0,
                    SupplierName = p.SupplierName
                }).ToList();
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving products", error = ex.Message });
            }
        }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public int StockQuantity { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; } = string.Empty;
    }
}
