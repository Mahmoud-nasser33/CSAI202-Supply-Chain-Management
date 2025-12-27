using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<ProductItem> ProductList { get; set; } = new List<ProductItem>();

        public ProductsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
           
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT ProductID, Product_Name, Price FROM Product";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductItem item = new ProductItem();
                            item.ProductID = reader.GetInt32(0);
                            item.Name = reader.GetString(1);
                            item.Price = reader.GetDecimal(2);
                            ProductList.Add(item);
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            
            string newName = Request.Form["NewProductName"];
            string newPriceStr = Request.Form["NewProductPrice"];

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Product (Product_Name, Price) VALUES (@Name, @Price)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", newName);
                    
                    decimal price = 0;
                    decimal.TryParse(newPriceStr, out price);
                    command.Parameters.AddWithValue("@Price", price);
                    
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
                
             
                string sqlDep1 = "DELETE FROM Inventory WHERE ProductID = @ID";
                using (SqlCommand cmd = new SqlCommand(sqlDep1, connection)) {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }

                string sqlDep2 = "DELETE FROM Order_Details WHERE ProductID = @ID";
                using (SqlCommand cmd = new SqlCommand(sqlDep2, connection)) {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }

                
                string sql = "DELETE FROM Product WHERE ProductID = @ID";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage();
        }
    }

    public class ProductItem
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
