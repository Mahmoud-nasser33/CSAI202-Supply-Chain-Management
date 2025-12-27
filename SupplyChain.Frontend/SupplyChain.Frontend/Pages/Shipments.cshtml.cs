using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class ShipmentsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<ShipmentItem> ShipmentList { get; set; } = new List<ShipmentItem>();

        public ShipmentsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            // ...
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                    SELECT 
                        s.ShipmentID, 
                        s.OrderID, 
                        w.Name, 
                        s.Status, 
                        s.Shipped_Via,
                        c.Name
                    FROM Shipment s
                    JOIN Warehouse w ON s.WarehouseID = w.WarehouseID
                    JOIN Purchase_Order po ON s.OrderID = po.OrderID
                    JOIN Customer c ON po.CustomerID = c.CustomerID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ShipmentItem item = new ShipmentItem();
                            item.ShipmentID = reader.GetInt32(0);
                            item.OrderID = reader.GetInt32(1);
                            item.WarehouseName = reader.GetString(2);
                            item.Status = reader.GetString(3);
                            if (!reader.IsDBNull(4)) item.ShippedVia = reader.GetString(4);
                            item.CustomerName = reader.GetString(5);
                            
                            ShipmentList.Add(item);
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            
            string orderIdStr = Request.Form["NewOrderID"];
            string whIdStr = Request.Form["NewWarehouseID"];
            string via = Request.Form["NewShippedVia"];

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Shipment (OrderID, WarehouseID, Shipped_Via) VALUES (@OrderID, @WarehouseID, @ShippedVia)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    int orderId = 0; int.TryParse(orderIdStr, out orderId);
                    command.Parameters.AddWithValue("@OrderID", orderId);
                    
                    int whId = 0; int.TryParse(whIdStr, out whId);
                    command.Parameters.AddWithValue("@WarehouseID", whId);
                    
                    command.Parameters.AddWithValue("@ShippedVia", via ?? (object)DBNull.Value);
                    
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
                string sql = "DELETE FROM Shipment WHERE ShipmentID = @ID";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage();
        }
    }

    public class ShipmentItem
    {
        public int ShipmentID { get; set; }
        public int OrderID { get; set; }
        public string WarehouseName { get; set; }
        public string Status { get; set; }
        public string ShippedVia { get; set; }
        public string CustomerName { get; set; }
    }
}
