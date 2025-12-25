// Interface defining database operations for Warehouse.
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public interface IWarehouseRepository
{
    Task<List<Warehouse>> GetAllWarehousesAsync();
    Task<Warehouse?> GetWarehouseByIdAsync(int id);
    Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse);
    Task<bool> UpdateWarehouseAsync(int id, Warehouse warehouse);
    Task<bool> DeleteWarehouseAsync(int id);
}

