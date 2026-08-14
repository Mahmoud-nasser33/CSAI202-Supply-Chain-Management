using Microsoft.Data.SqlClient;
using SilsilaSupply.Models;

namespace SilsilaSupply.Services
{
    public class SupplierService : ServiceBase
    {
        public SupplierService(IConfiguration configuration, ILogger<SupplierService> logger)
            : base(configuration, logger)
        {
        }

        public DataResult<List<Supplier>> GetAll()
        {
            try
            {
                var suppliers = new List<Supplier>();
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT SupplierID, Name, Email, Rating FROM Supplier ORDER BY Name";
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    suppliers.Add(new Supplier
                    {
                        SupplierId = reader.GetInt32(0),
                        Name = reader.GetStringOrNull(1),
                        Email = reader.GetStringOrNull(2),
                        Rating = reader.GetDecimalOrNull(3)
                    });
                }
                return DataResult<List<Supplier>>.Ok(suppliers);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load suppliers.");
                return DataResult<List<Supplier>>.Fail(ResolveErrorMessage(ex));
            }
        }

        public DataResult<Supplier> GetById(int supplierId)
        {
            try
            {
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT SupplierID, Name, Email, Rating FROM Supplier WHERE SupplierID = @ID";
                command.Parameters.AddWithValue("@ID", supplierId);
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return DataResult<Supplier>.Ok(new Supplier
                    {
                        SupplierId = reader.GetInt32(0),
                        Name = reader.GetStringOrNull(1),
                        Email = reader.GetStringOrNull(2),
                        Rating = reader.GetDecimalOrNull(3)
                    });
                }
                return DataResult<Supplier>.Fail("The supplier could not be found.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load supplier {SupplierId}.", supplierId);
                return DataResult<Supplier>.Fail(ResolveErrorMessage(ex));
            }
        }

        public OperationResult Create(SupplierInput input)
        {
            try
            {
                ExecuteNonQuery(
                    "INSERT INTO Supplier (Name, Email, Rating) VALUES (@Name, @Email, @Rating)",
                    ("@Name", input.Name),
                    ("@Email", input.Email),
                    ("@Rating", input.Rating));
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create supplier.");
                return OperationResult.Fail(
                    ResolveErrorMessage(ex, duplicateMessage: "A supplier with this email already exists."));
            }
        }

        public OperationResult Update(int supplierId, SupplierInput input)
        {
            try
            {
                int affected = ExecuteNonQuery(
                    "UPDATE Supplier SET Name = @Name, Email = @Email, Rating = @Rating WHERE SupplierID = @ID",
                    ("@Name", input.Name),
                    ("@Email", input.Email),
                    ("@Rating", input.Rating),
                    ("@ID", supplierId));
                if (affected == 0)
                {
                    return OperationResult.Fail("The supplier could not be found.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update supplier {SupplierId}.", supplierId);
                return OperationResult.Fail(
                    ResolveErrorMessage(ex, duplicateMessage: "A supplier with this email already exists."));
            }
        }

        public OperationResult Delete(int supplierId)
        {
            try
            {
                int affected = ExecuteInTransaction((connection, transaction) =>
                {
                    List<int> productIds = new List<int>();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "SELECT ProductID FROM Product WHERE SupplierID = @ID";
                        command.Parameters.AddWithValue("@ID", supplierId);
                        using SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            productIds.Add(reader.GetInt32(0));
                        }
                    }

                    foreach (int productId in productIds)
                    {
                        ExecuteNonQuery(connection, transaction,
                            "DELETE FROM Inventory WHERE ProductID = @PID", ("@PID", productId));
                        ExecuteNonQuery(connection, transaction,
                            "DELETE FROM Order_Details WHERE ProductID = @PID", ("@PID", productId));
                        ExecuteNonQuery(connection, transaction,
                            "DELETE FROM Product WHERE ProductID = @PID", ("@PID", productId));
                    }

                    return ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Supplier WHERE SupplierID = @ID", ("@ID", supplierId));
                });
                if (affected == 0)
                {
                    return OperationResult.Fail("The supplier no longer exists.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete supplier {SupplierId}.", supplierId);
                return OperationResult.Fail(ResolveErrorMessage(ex));
            }
        }
    }
}
