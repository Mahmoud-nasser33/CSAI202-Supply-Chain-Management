using Microsoft.Data.SqlClient;
using SilsilaSupply.Models;

namespace SilsilaSupply.Services
{
    public class WarehouseService : ServiceBase
    {
        public WarehouseService(IConfiguration configuration, ILogger<WarehouseService> logger)
            : base(configuration, logger)
        {
        }

        public DataResult<List<Warehouse>> GetAll()
        {
            try
            {
                var warehouses = new List<Warehouse>();
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT WarehouseID, Name, Location FROM Warehouse ORDER BY Name";
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    warehouses.Add(new Warehouse
                    {
                        WarehouseId = reader.GetInt32(0),
                        Name = reader.GetStringOrNull(1),
                        Location = reader.GetStringOrNull(2)
                    });
                }
                return DataResult<List<Warehouse>>.Ok(warehouses);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load warehouses.");
                return DataResult<List<Warehouse>>.Fail(ResolveErrorMessage(ex));
            }
        }
    }
}
