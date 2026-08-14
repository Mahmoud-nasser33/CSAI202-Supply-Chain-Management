using Microsoft.Data.SqlClient;
using SilsilaSupply.Models;

namespace SilsilaSupply.Services
{
    public class ProductService : ServiceBase
    {
        public ProductService(IConfiguration configuration, ILogger<ProductService> logger)
            : base(configuration, logger)
        {
        }

        public DataResult<List<Product>> GetAll()
        {
            try
            {
                var products = new List<Product>();
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ProductID, Product_Name, Price FROM Product ORDER BY Product_Name";
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetStringOrNull(1),
                        Price = reader.GetDecimal(2)
                    });
                }
                return DataResult<List<Product>>.Ok(products);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load products.");
                return DataResult<List<Product>>.Fail(ResolveErrorMessage(ex));
            }
        }

        public DataResult<Product> GetById(int productId)
        {
            try
            {
                using SqlConnection connection = CreateConnection();
                connection.Open();
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ProductID, Product_Name, Price FROM Product WHERE ProductID = @ID";
                command.Parameters.AddWithValue("@ID", productId);
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return DataResult<Product>.Ok(new Product
                    {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetStringOrNull(1),
                        Price = reader.GetDecimal(2)
                    });
                }
                return DataResult<Product>.Fail("The product could not be found.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load product {ProductId}.", productId);
                return DataResult<Product>.Fail(ResolveErrorMessage(ex));
            }
        }

        public OperationResult Create(ProductInput input)
        {
            try
            {
                ExecuteNonQuery(
                    "INSERT INTO Product (Product_Name, Price) VALUES (@Name, @Price)",
                    ("@Name", input.Name),
                    ("@Price", input.Price));
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create product.");
                return OperationResult.Fail(ResolveErrorMessage(ex));
            }
        }

        public OperationResult Update(int productId, ProductInput input)
        {
            try
            {
                int affected = ExecuteNonQuery(
                    "UPDATE Product SET Product_Name = @Name, Price = @Price WHERE ProductID = @ID",
                    ("@Name", input.Name),
                    ("@Price", input.Price),
                    ("@ID", productId));
                if (affected == 0)
                {
                    return OperationResult.Fail("The product could not be found.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update product {ProductId}.", productId);
                return OperationResult.Fail(ResolveErrorMessage(ex));
            }
        }

        public OperationResult Delete(int productId)
        {
            try
            {
                int affected = ExecuteInTransaction((connection, transaction) =>
                {
                    ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Inventory WHERE ProductID = @ID", ("@ID", productId));
                    ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Order_Details WHERE ProductID = @ID", ("@ID", productId));
                    return ExecuteNonQuery(connection, transaction,
                        "DELETE FROM Product WHERE ProductID = @ID", ("@ID", productId));
                });
                if (affected == 0)
                {
                    return OperationResult.Fail("The product no longer exists.");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete product {ProductId}.", productId);
                return OperationResult.Fail(ResolveErrorMessage(ex));
            }
        }
    }
}
