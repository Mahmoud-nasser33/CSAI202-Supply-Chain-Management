// Manages HTTP requests and API logic for Orders.
﻿using Microsoft.AspNetCore.Mvc;
using SupplyChain.Backend.Models;
using SupplyChain.Backend.Repositories;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                List<Order> orders = await _orderRepository.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving orders", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = $"Order with ID {id} not found" });
                }

                var orderDetails = await _orderRepository.GetOrderDetailsAsync(id);
                return Ok(new { Order = order, Details = orderDetails });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving order", error = ex.Message });
            }
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomer(int customerId)
        {
            try
            {
                List<Order> orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving customer orders", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var order = new Order
                {
                    CustomerID = request.CustomerID,
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    TotalAmount = request.OrderDetails.Sum(d => d.Quantity * d.UnitPrice)
                };

                var orderDetails = request.OrderDetails.Select(d => new OrderDetail
                {
                    ProductID = d.ProductID,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList();

                var createdOrder = await _orderRepository.CreateOrderAsync(order, orderDetails);
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderID }, createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating order", error = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                bool updated = await _orderRepository.UpdateOrderStatusAsync(id, request.Status);
                if (!updated)
                {
                    return NotFound(new { message = $"Order with ID {id} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating order status", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                bool deleted = await _orderRepository.DeleteOrderAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = $"Order with ID {id} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting order", error = ex.Message });
            }
        }
    }

    public class CreateOrderRequest
    {
        public int CustomerID { get; set; }
        public List<OrderDetailRequest> OrderDetails { get; set; } = new();
    }

    public class OrderDetailRequest
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }
}