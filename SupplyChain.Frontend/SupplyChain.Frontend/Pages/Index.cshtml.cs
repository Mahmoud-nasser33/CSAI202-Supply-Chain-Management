using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace SupplyChain.Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int TotalProducts = 0;
        public int TotalCustomers = 0;

        public void OnGet()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlProducts = "SELECT COUNT(*) FROM Product";
                using (SqlCommand cmd = new SqlCommand(sqlProducts, connection))
                {
                    TotalProducts = (int)cmd.ExecuteScalar();
                }

                string sqlCustomers = "SELECT COUNT(*) FROM Customer";
                using (SqlCommand cmd = new SqlCommand(sqlCustomers, connection))
                {
                    TotalCustomers = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
