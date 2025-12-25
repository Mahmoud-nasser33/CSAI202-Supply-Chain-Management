// Interface defining database operations for User.
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(int id, User user);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> ValidateUserAsync(string email, string password);
}

