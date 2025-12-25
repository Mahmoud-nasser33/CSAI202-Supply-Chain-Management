using SupplyChain.Backend.Models;
// Defines the ProductDto class/logic for the Supply Chain system.
﻿namespace SupplyChain.Frontend.Pages
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Supplier { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public string Description { get; set; }
    }
}