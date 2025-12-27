using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class CustomersModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<CustomerItem> CustomerList { get; set; } = new List<CustomerItem>();

        public CustomersModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT CustomerID, Name, Email FROM Customer";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerItem customer = new CustomerItem();
                            customer.CustomerID = reader.GetInt32(0);
                            customer.Name = reader.GetString(1);
                            customer.Email = reader.GetString(2);
                            CustomerList.Add(customer);
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            
            string newName = Request.Form["NewCustomerName"];
            string newEmail = Request.Form["NewCustomerEmail"];

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Customer (Name, Email) VALUES (@Name, @Email)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", newName);
                    command.Parameters.AddWithValue("@Email", newEmail);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
              
                List<int> orderIds = new List<int>();
                string getOrders = "SELECT OrderID FROM Purchase_Order WHERE CustomerID = @ID";
                using (SqlCommand cmd = new SqlCommand(getOrders, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read()) orderIds.Add(reader.GetInt32(0));
                    }
                }

               
                foreach(int orderId in orderIds)
                {
                    string[] orderDeps = { 
                        "DELETE FROM Order_Details WHERE OrderID = @OID",
                        "DELETE FROM Shipment WHERE OrderID = @OID",
                        "DELETE FROM Payment WHERE OrderID = @OID"
                    };
                    foreach(string depSql in orderDeps) {
                        using (SqlCommand depCmd = new SqlCommand(depSql, connection)) {
                            depCmd.Parameters.AddWithValue("@OID", orderId);
                            depCmd.ExecuteNonQuery();
                        }
                    }
                    
               
                    using (SqlCommand delOrder = new SqlCommand("DELETE FROM Purchase_Order WHERE OrderID = @OID", connection)) {
                        delOrder.Parameters.AddWithValue("@OID", orderId);
                        delOrder.ExecuteNonQuery();
                    }
                }

                
                string sql = "DELETE FROM Customer WHERE CustomerID = @ID";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage();
        }
    }

    public class CustomerItem
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
