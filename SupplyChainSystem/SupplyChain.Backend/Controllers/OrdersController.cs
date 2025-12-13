using Microsoft.AspNetCore.Mvc;
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        
        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (order.TotalAmount <= 0)
            {
                return BadRequest("Order total amount must be greater than zero.");
            }

            if (order.TotalAmount > 100000)
            {
                // Simulate server error for large orders
                return StatusCode(500, "Simulated backend error: Order limit exceeded.");
            }
            
            return Ok(new
            {
                Message = "Order #105 Created Successfully",
                Status = "Pending",
                Date = DateTime.Now,
                Total = order.TotalAmount
            });
        }

        
        [HttpGet("history/{userId}")]
        public IActionResult GetHistory(int userId)
        {
            var history = new List<Order>
            {
                new Order { OrderID = 101, CustomerID = userId, Status = "Shipped", TotalAmount = 15000 },
                new Order { OrderID = 102, CustomerID = userId, Status = "Processing", TotalAmount = 450 }
            };
            return Ok(history);
        }
    }
}