// Interface defining database operations for Supplier.
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllSuppliersAsync();
    Task<Supplier?> GetSupplierByIdAsync(int id);
    Task<Supplier> CreateSupplierAsync(Supplier supplier);
    Task<bool> UpdateSupplierAsync(int id, Supplier supplier);
    Task<bool> DeleteSupplierAsync(int id);
}

