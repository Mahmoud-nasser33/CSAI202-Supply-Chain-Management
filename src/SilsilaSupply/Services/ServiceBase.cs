using Microsoft.Data.SqlClient;

namespace SilsilaSupply.Services
{
    public abstract class ServiceBase
    {
        private readonly IConfiguration _configuration;
        protected ILogger Logger { get; }

        protected ServiceBase(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            Logger = logger;
        }

        protected SqlConnection CreateConnection()
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("The database connection string 'DefaultConnection' is not configured.");
            }
            return new SqlConnection(connectionString);
        }

        protected static void AddParameters(SqlCommand command, params (string Name, object? Value)[] parameters)
        {
            foreach (var (name, value) in parameters)
            {
                command.Parameters.AddWithValue(name, value ?? DBNull.Value);
            }
        }

        protected int ExecuteNonQuery(string sql, params (string Name, object? Value)[] parameters)
        {
            using SqlConnection connection = CreateConnection();
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            AddParameters(command, parameters);
            return command.ExecuteNonQuery();
        }

        protected static int ExecuteNonQuery(SqlConnection connection, SqlTransaction transaction, string sql, params (string Name, object? Value)[] parameters)
        {
            using SqlCommand command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = sql;
            AddParameters(command, parameters);
            return command.ExecuteNonQuery();
        }

        protected static object? ExecuteScalar(SqlConnection connection, SqlTransaction transaction, string sql, params (string Name, object? Value)[] parameters)
        {
            using SqlCommand command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = sql;
            AddParameters(command, parameters);
            return command.ExecuteScalar();
        }

        protected void ExecuteInTransaction(Action<SqlConnection, SqlTransaction> action)
        {
            ExecuteInTransaction<int>((connection, transaction) =>
            {
                action(connection, transaction);
                return 0;
            });
        }

        protected TResult ExecuteInTransaction<TResult>(Func<SqlConnection, SqlTransaction, TResult> action)
        {
            using SqlConnection connection = CreateConnection();
            connection.Open();
            using SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                TResult result = action(connection, transaction);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        protected static string ResolveErrorMessage(Exception ex, string? duplicateMessage = null, string? referencedMessage = null)
        {
            if (ex is SqlException sqlEx)
            {
                if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                {
                    return duplicateMessage ?? "This record already exists — one of the values you entered is already in use.";
                }
                if (sqlEx.Number == 547)
                {
                    return referencedMessage ?? "This record cannot be changed because other records still refer to it.";
                }
            }
            if (ex is InvalidOperationException)
            {
                return "The database could not be reached. Please try again.";
            }
            return "Something went wrong. Please try again later.";
        }
    }
}
