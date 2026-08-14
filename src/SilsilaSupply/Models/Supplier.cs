using System.ComponentModel.DataAnnotations;

namespace SilsilaSupply.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public decimal? Rating { get; set; }
    }

    public class SupplierInput
    {
        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(255, ErrorMessage = "Supplier name cannot exceed 255 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(255, ErrorMessage = "Email address cannot exceed 255 characters.")]
        public string? Email { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public decimal? Rating { get; set; }
    }
}
