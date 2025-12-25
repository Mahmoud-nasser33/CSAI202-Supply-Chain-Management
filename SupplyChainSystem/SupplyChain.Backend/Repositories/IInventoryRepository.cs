// Interface defining database operations for Inventory.
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public interface IInventoryRepository
{
    Task<List<Inventory>> GetAllInventoryAsync();
    Task<Inventory?> GetInventoryByIdAsync(int id);
    Task<Inventory> CreateInventoryAsync(Inventory inventory);
    Task<bool> UpdateInventoryAsync(int id, Inventory inventory);
    Task<bool> DeleteInventoryAsync(int id);
}

