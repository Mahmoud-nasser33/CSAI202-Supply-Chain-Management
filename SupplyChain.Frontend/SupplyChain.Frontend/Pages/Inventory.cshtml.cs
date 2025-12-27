using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class InventoryModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<InventoryItem> InventoryList { get; set; } = new List<InventoryItem>();

        public InventoryModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                    SELECT 
                        i.InventoryID, 
                        p.Product_Name, 
                        w.Name, 
                        i.Quantity_Available, 
                        i.LastUpdated 
                    FROM Inventory i
                    JOIN Product p ON i.ProductID = p.ProductID
                    JOIN Warehouse w ON i.WarehouseID = w.WarehouseID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InventoryItem item = new InventoryItem();
                            item.InventoryID = reader.GetInt32(0);
                            item.ProductName = reader.GetString(1);
                            item.WarehouseName = reader.GetString(2);
                            item.Quantity = reader.GetInt32(3);
                            item.LastUpdated = reader.GetDateTime(4);
                            InventoryList.Add(item);
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
          
            string prodIdStr = Request.Form["NewProductID"];
            string whIdStr = Request.Form["NewWarehouseID"];
            string qtyStr = Request.Form["NewQuantity"];

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Inventory (ProductID, WarehouseID, Quantity_Available) VALUES (@ProductID, @WarehouseID, @Quantity)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    int pid = 0; int.TryParse(prodIdStr, out pid);
                    command.Parameters.AddWithValue("@ProductID", pid);
                    
                    int wid = 0; int.TryParse(whIdStr, out wid);
                    command.Parameters.AddWithValue("@WarehouseID", wid);
                    
                    int qty = 0; int.TryParse(qtyStr, out qty);
                    command.Parameters.AddWithValue("@Quantity", qty);
                    
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
                string sql = "DELETE FROM Inventory WHERE InventoryID = @ID";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage();
        }
    }

    public class InventoryItem
    {
        public int InventoryID { get; set; }
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
