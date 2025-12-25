// Handles database data access operations for User.
using Microsoft.Data.SqlClient;
using SupplyChain.Backend.Data;
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT u.UserID, u.User_Name, u.Email, u.Password, u.RoleID, r.Role_Name
            FROM [User] u
            LEFT JOIN [Role] r ON u.RoleID = r.RoleID
            ORDER BY u.UserID";

        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        List<User> users = new List<User>();
        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                UserID = reader.GetInt32(0),
                User_Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                RoleID = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                Role_Name = reader.IsDBNull(5) ? null : reader.GetString(5)
            });
        }

        return users;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT u.UserID, u.User_Name, u.Email, u.Password, u.RoleID, r.Role_Name
            FROM [User] u
            LEFT JOIN [Role] r ON u.RoleID = r.RoleID
            WHERE u.Email = @Email";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", email);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                UserID = reader.GetInt32(0),
                User_Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                RoleID = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                Role_Name = reader.IsDBNull(5) ? null : reader.GetString(5)
            };
        }

        return null;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT u.UserID, u.User_Name, u.Email, u.Password, u.RoleID, r.Role_Name
            FROM [User] u
            LEFT JOIN [Role] r ON u.RoleID = r.RoleID
            WHERE u.UserID = @UserID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserID", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                UserID = reader.GetInt32(0),
                User_Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                RoleID = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                Role_Name = reader.IsDBNull(5) ? null : reader.GetString(5)
            };
        }

        return null;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            INSERT INTO [User] (User_Name, Email, Password, RoleID)
            OUTPUT INSERTED.UserID
            VALUES (@User_Name, @Email, @Password, @RoleID)";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@User_Name", user.User_Name);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.Parameters.AddWithValue("@RoleID", (object?)user.RoleID ?? DBNull.Value);

        var newId = (int)await command.ExecuteScalarAsync();
        user.UserID = newId;
        return user;
    }

    public async Task<bool> UpdateUserAsync(int id, User user)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            UPDATE [User]
            SET User_Name = @User_Name, Email = @Email, Password = @Password, RoleID = @RoleID
            WHERE UserID = @UserID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserID", id);
        command.Parameters.AddWithValue("@User_Name", user.User_Name);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.Parameters.AddWithValue("@RoleID", (object?)user.RoleID ?? DBNull.Value);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"DELETE FROM [User] WHERE UserID = @UserID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserID", id);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> ValidateUserAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);
        if (user == null) return false;

        return user.Password == password;
    }
}

