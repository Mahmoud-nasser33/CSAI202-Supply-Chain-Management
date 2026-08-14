using System.ComponentModel.DataAnnotations;

namespace SilsilaSupply.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmount { get; set; }
    }

    public class OrderInput
    {
        [Range(1, int.MaxValue, ErrorMessage = "Select a customer.")]
        public int CustomerId { get; set; }

        [Range(0, 99999999.99, ErrorMessage = "Total amount must be between 0 and 99,999,999.99.")]
        public decimal TotalAmount { get; set; }

        [StringLength(100, ErrorMessage = "Status cannot exceed 100 characters.")]
        public string? Status { get; set; }
    }
}
