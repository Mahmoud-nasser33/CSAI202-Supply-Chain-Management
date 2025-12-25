// Handles database data access operations for Customer.
using Microsoft.Data.SqlClient;
using SupplyChain.Backend.Data;
using SupplyChain.Backend.Models;

namespace SupplyChain.Backend.Repositories;
public class CustomerRepository : ICustomerRepository
{
    private readonly DatabaseContext _context;

    public CustomerRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        List<Customer> customers = new List<Customer>();
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT c.CustomerID, c.Name, c.Email, c.Phone, c.Address, c.City, c.Country,
                   (SELECT COUNT(*) FROM Purchase_Order WHERE CustomerID = c.CustomerID) as TotalOrders
            FROM Customer c";

        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            customers.Add(new Customer
            {
                CustomerID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                Address = reader.IsDBNull(4) ? null : reader.GetString(4),
                City = reader.IsDBNull(5) ? null : reader.GetString(5),
                Country = reader.IsDBNull(6) ? null : reader.GetString(6),
                TotalOrders = reader.GetInt32(7)
            });
        }

        return customers;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"SELECT CustomerID, Name, Email, Phone, Address, City, Country FROM Customer WHERE CustomerID = @CustomerID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CustomerID", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Customer
            {
                CustomerID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                Address = reader.IsDBNull(4) ? null : reader.GetString(4),
                City = reader.IsDBNull(5) ? null : reader.GetString(5),
                Country = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }

        return null;
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            INSERT INTO Customer (Name, Email, Phone, Address, City, Country)
            OUTPUT INSERTED.CustomerID
            VALUES (@Name, @Email, @Phone, @Address, @City, @Country)";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", customer.Name);
        command.Parameters.AddWithValue("@Email", (object?)customer.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Phone", (object?)customer.Phone ?? DBNull.Value);
        command.Parameters.AddWithValue("@Address", (object?)customer.Address ?? DBNull.Value);
        command.Parameters.AddWithValue("@City", (object?)customer.City ?? DBNull.Value);
        command.Parameters.AddWithValue("@Country", (object?)customer.Country ?? DBNull.Value);

        int id = (int)await command.ExecuteScalarAsync();
        customer.CustomerID = id;

        return customer;
    }

    public async Task<bool> UpdateCustomerAsync(int id, Customer customer)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            UPDATE Customer
            SET Name = @Name,
                Email = @Email,
                Phone = @Phone,
                Address = @Address,
                City = @City,
                Country = @Country
            WHERE CustomerID = @CustomerID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CustomerID", id);
        command.Parameters.AddWithValue("@Name", customer.Name);
        command.Parameters.AddWithValue("@Email", (object?)customer.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Phone", (object?)customer.Phone ?? DBNull.Value);
        command.Parameters.AddWithValue("@Address", (object?)customer.Address ?? DBNull.Value);
        command.Parameters.AddWithValue("@City", (object?)customer.City ?? DBNull.Value);
        command.Parameters.AddWithValue("@Country", (object?)customer.Country ?? DBNull.Value);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        using SqlConnection connection = _context.CreateConnection();
        await connection.OpenAsync();

        string query = @"DELETE FROM Customer WHERE CustomerID = @CustomerID";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CustomerID", id);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}

