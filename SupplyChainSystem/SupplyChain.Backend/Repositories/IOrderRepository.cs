// Interface defining database operations for Order.
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public interface IOrderRepository
{
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
    Task<Order> CreateOrderAsync(Order order, List<OrderDetail> orderDetails);
    Task<bool> UpdateOrderStatusAsync(int id, string status);
    Task<bool> DeleteOrderAsync(int id);
    Task<List<OrderDetail>> GetOrderDetailsAsync(int orderId);
}

