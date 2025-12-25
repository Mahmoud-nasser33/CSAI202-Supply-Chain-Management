// Represents the Category entity in the database.
namespace SupplyChain.Backend.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
