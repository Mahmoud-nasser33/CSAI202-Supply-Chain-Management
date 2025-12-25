// Defines the DatabaseContext class/logic for the Supply Chain system.
using Microsoft.Data.SqlClient;
using System.Data;

namespace SupplyChain.Backend.Data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public DatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using SqlConnection connection = CreateConnection();
                await connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
