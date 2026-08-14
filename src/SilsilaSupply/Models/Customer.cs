using System.ComponentModel.DataAnnotations;

namespace SilsilaSupply.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

    public class CustomerInput
    {
        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(255, ErrorMessage = "Customer name cannot exceed 255 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(255, ErrorMessage = "Email address cannot exceed 255 characters.")]
        public string? Email { get; set; }
    }
}
