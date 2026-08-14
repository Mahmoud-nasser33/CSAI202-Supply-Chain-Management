using System.ComponentModel.DataAnnotations;

namespace SilsilaSupply.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductInput
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(255, ErrorMessage = "Product name cannot exceed 255 characters.")]
        public string? Name { get; set; }

        [Range(0.01, 99999999.99, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
    }
}
