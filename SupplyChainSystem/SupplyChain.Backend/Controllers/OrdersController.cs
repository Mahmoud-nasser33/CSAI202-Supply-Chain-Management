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