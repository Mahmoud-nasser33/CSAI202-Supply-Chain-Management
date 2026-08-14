using Microsoft.Data.SqlClient;
using SilsilaSupply.Models;

namespace SilsilaSupply.Services
{
    public class CustomerService : ServiceBase
    {
        public CustomerService(IConfiguration configuration, ILogger<CustomerService> logger)
            : base(configuration, logger)
        {
        }

        public DataResult<List<Customer>> GetAll()
        {
            try
            {
                var customers = new List<Customer>();
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT CustomerID, Name, Email FROM Customer ORDER BY Name";
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        CustomerId = reader.GetInt32(0),
                        Name = reader.GetStringOrNull(1),
                        Email = reader.GetStringOrNull(2)
                    });
                }
                return DataResult<List<Customer>>.Ok(customers);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load customers.");
                return DataResult<List<Customer>>.Fail(ResolveErrorMessage(ex));
            }
        }

        public DataResult<Customer> GetById(int customerId)
        {
            try
            {
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT CustomerID, Name, Email FROM Customer WHERE CustomerID = @ID";
                command.Parameters.AddWithValue("@ID", customerId);
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return DataResult<Customer>.Ok(new Customer
                    {
                        CustomerId = reader.GetInt32(0),
                        Name = reader.GetStringOrNull(1),
                        Email = reader.GetStringOrNull(2)
                    });
                }
                return DataResult<Customer>.Fail("The customer could not be found.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load customer {CustomerId}.", customerId);
                return DataResult<Customer>.Fail(ResolveErrorMessage(ex));
            }
        }

        public OperationResult Create(CustomerInput input)
        {
            try
            {
                ExecuteNonQuery(
                    "INSERT INTO Customer (Name, Email) VALUES (@Name, @Email)",
                    ("@Name", input.Name),
                    ("@Email", input.Email));
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create customer.");
                return OperationResult.Fail(
                    ResolveErrorMessage(ex, duplicateMessage: "A customer with this email already exists."));
            }
        }

        public OperationResult Update(int customerId, CustomerInput input)
        {
            try
            {
                int affected = ExecuteNonQuery(
                    "UPDATE Customer SET Name = @Name, Email = @Email WHERE CustomerID = @ID",
                    ("@Name", input.Name),
                    ("@Email", input.Email),
                    ("@ID", customerId));
                if (affected == 0)
                {
                    return OperationResult.Fail("The customer could not be found.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update customer {CustomerId}.", customerId);
                return OperationResult.Fail(
                    ResolveErrorMessage(ex, duplicateMessage: "A customer with this email already exists."));
            }
        }

        public OperationResult Delete(int customerId)
        {
            try
            {
                int affected = ExecuteInTransaction((connection, transaction) =>
                {
                    ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Feedback WHERE CustomerID = @ID", ("@ID", customerId));
                    ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Order_Details WHERE OrderID IN (SELECT OrderID FROM Purchase_Order WHERE CustomerID = @ID)", ("@ID", customerId));
                    ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Shipment WHERE OrderID IN (SELECT OrderID FROM Purchase_Order WHERE CustomerID = @ID)", ("@ID", customerId));
                    ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Payment WHERE OrderID IN (SELECT OrderID FROM Purchase_Order WHERE CustomerID = @ID)", ("@ID", customerId));
                    ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Purchase_Order WHERE CustomerID = @ID", ("@ID", customerId));
                    return ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Customer WHERE CustomerID = @ID", ("@ID", customerId));
                });
                if (affected == 0)
                {
                    return OperationResult.Fail("The customer no longer exists.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete customer {CustomerId}.", customerId);
                return OperationResult.Fail(ResolveErrorMessage(ex));
            }
        }
    }
}
