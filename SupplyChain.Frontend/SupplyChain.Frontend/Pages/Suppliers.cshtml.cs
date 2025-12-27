using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class SuppliersModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<SupplierItem> SupplierList { get; set; } = new List<SupplierItem>();

        public SuppliersModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT SupplierID, Name, Email, Rating FROM Supplier";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SupplierItem item = new SupplierItem();
                            item.SupplierID = reader.GetInt32(0);
                            item.Name = reader.GetString(1);
                            item.Email = reader.GetString(2);
                          
                            if (!reader.IsDBNull(3))
                            {
                                item.Rating = reader.GetDecimal(3);
                            }
                            SupplierList.Add(item);
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            
            string newName = Request.Form["NewSupplierName"];
            string newEmail = Request.Form["NewSupplierEmail"];
            string newRatingStr = Request.Form["NewSupplierRating"];

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Supplier (Name, Email, Rating) VALUES (@Name, @Email, @Rating)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", newName);
                    command.Parameters.AddWithValue("@Email", newEmail);
                
                    decimal rating = 0;
                    decimal.TryParse(newRatingStr, out rating);
                    command.Parameters.AddWithValue("@Rating", rating);
                    
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
                
               
                List<int> productIds = new List<int>();
                string getProds = "SELECT ProductID FROM Product WHERE SupplierID = @ID";
                using (SqlCommand cmd = new SqlCommand(getProds, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read()) productIds.Add(reader.GetInt32(0));
                    }
                }

                
                foreach(int pid in productIds)
                {
                    string[] prodDeps = { 
                        "DELETE FROM Inventory WHERE ProductID = @PID",
                        "DELETE FROM Order_Details WHERE ProductID = @PID"
                    };
                    foreach(string depSql in prodDeps) {
                        using (SqlCommand depCmd = new SqlCommand(depSql, connection)) {
                            depCmd.Parameters.AddWithValue("@PID", pid);
                            depCmd.ExecuteNonQuery();
                        }
                    }
                    
                   
                    using (SqlCommand delProd = new SqlCommand("DELETE FROM Product WHERE ProductID = @PID", connection)) {
                        delProd.Parameters.AddWithValue("@PID", pid);
                        delProd.ExecuteNonQuery();
                    }
                }

               
                string sql = "DELETE FROM Supplier WHERE SupplierID = @ID";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage();
        }
    }

    public class SupplierItem
    {
        public int SupplierID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Rating { get; set; }
    }
}
