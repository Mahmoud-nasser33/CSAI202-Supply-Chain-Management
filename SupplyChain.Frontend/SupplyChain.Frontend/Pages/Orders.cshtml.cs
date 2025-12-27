using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace SupplyChain.Frontend.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<OrderItem> OrderList { get; set; } = new List<OrderItem>();

        public OrdersModel(IConfiguration configuration)
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
                        po.OrderID, 
                        c.Name, 
                        po.OrderDate, 
                        po.Status, 
                        po.TotalAmount 
                    FROM Purchase_Order po
                    JOIN Customer c ON po.CustomerID = c.CustomerID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderItem item = new OrderItem();
                            item.OrderID = reader.GetInt32(0);
                            item.CustomerName = reader.GetString(1);
                            item.OrderDate = reader.GetDateTime(2);
                            item.Status = reader.GetString(3);
                            item.TotalAmount = reader.GetDecimal(4);
                            OrderList.Add(item);
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
          
            string custIdStr = Request.Form["NewCustomerID"];
            string totalStr = Request.Form["NewOrderTotal"];

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Purchase_Order (CustomerID, TotalAmount) VALUES (@CustomerID, @TotalAmount)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    int custId = 0;
                    int.TryParse(custIdStr, out custId);
                    command.Parameters.AddWithValue("@CustomerID", custId);
                    
                    decimal total = 0;
                    decimal.TryParse(totalStr, out total);
                    command.Parameters.AddWithValue("@TotalAmount", total);
                    
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
                
                
                string[] dependencies = { 
                    "DELETE FROM Order_Details WHERE OrderID = @ID",
                    "DELETE FROM Shipment WHERE OrderID = @ID",
                    "DELETE FROM Payment WHERE OrderID = @ID"
                };

                foreach (string sqlDep in dependencies)
                {
                    using (SqlCommand cmd = new SqlCommand(sqlDep, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.ExecuteNonQuery();
                    }
                }

           
                string sql = "DELETE FROM Purchase_Order WHERE OrderID = @ID";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage();
        }
    }

    public class OrderItem
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
