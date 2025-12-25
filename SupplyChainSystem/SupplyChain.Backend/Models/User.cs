// Represents the User entity in the database.
namespace SupplyChain.Backend.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string User_Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? RoleID { get; set; }
        public string? Role_Name { get; set; }
    }
}
