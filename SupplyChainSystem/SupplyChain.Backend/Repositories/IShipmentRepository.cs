// Interface defining database operations for Shipment.
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public interface IShipmentRepository
{
    Task<List<Shipment>> GetAllShipmentsAsync();
    Task<Shipment?> GetShipmentByIdAsync(int id);
    Task<Shipment> CreateShipmentAsync(Shipment shipment);
    Task<bool> UpdateShipmentAsync(int id, Shipment shipment);
    Task<bool> DeleteShipmentAsync(int id);
}

