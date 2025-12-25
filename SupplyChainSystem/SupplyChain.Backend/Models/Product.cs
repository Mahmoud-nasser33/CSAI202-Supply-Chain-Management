// Represents the Product entity in the database.
﻿namespace SupplyChain.Backend.Models
{
    public class Product
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public int? SupplierID { get; set; }
        public int StockQuantity { get; set; }
        public string SupplierName { get; set; } = string.Empty;
    }
}