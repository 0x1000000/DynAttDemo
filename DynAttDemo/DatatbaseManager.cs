using System.Data.SqlClient;
using System.Reflection;

namespace DynAttDemo
{
    internal class DatatbaseManager : IDisposable
    {
        public DatatbaseManager(string? connectionString, string? errorText, bool created)
        {
            ConnectionString = connectionString;
            ErrorText = errorText;
            Created = created;
        }

        public string? ConnectionString { get; private set; }

        public string? ErrorText { get; private set; }

        public bool Created { get; private set; }

        public static DatatbaseManager CreateDatabaseFile(string server, string databaseName)
        {
            var currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
            var databasePath = Path.Combine(currentDir!, databaseName);

            var connectionString = $"Server={server};Integrated security=SSPI;database=master";
            var resultConnectionString = $"{connectionString };Initial Catalog={databaseName}";

            using var connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                return new DatatbaseManager(null, $"Could not connect to SQL server {server}. " + ex.Message, false);
            }

            if (CheckDatabaseExists(databaseName, connection))
            {
                return new DatatbaseManager(resultConnectionString, null, false);
            }

            var commandText = $"CREATE DATABASE {databaseName} ON PRIMARY " +
             $"(NAME = {databaseName}, " +
             $"FILENAME = '{databasePath}.mdf', " +
             "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)" +
             $"LOG ON (NAME = {databaseName}_Log, " +
             $"FILENAME = '{databasePath}', " +
             "SIZE = 1MB, MAXSIZE = 5MB, FILEGROWTH = 10%)";

            using var command = new SqlCommand(commandText, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return new DatatbaseManager(null, $"Could not create {databaseName}. " + ex.Message, false);
            }

            return new DatatbaseManager(resultConnectionString, null, true);
        }

        private static bool CheckDatabaseExists(string databaseName, SqlConnection connection)
        {
            var checkCommandText = $"IF DB_ID('{databaseName}') IS NOT NULL SELECT 1 ELSE SELECT 0";

            using var checkCommand = new SqlCommand(checkCommandText, connection);

            return Equals(checkCommand.ExecuteScalar(), 1);
        }

        public void Dispose()
        {
            if(this.Created && this.ConnectionString != null)
            {
                var builder = new SqlConnectionStringBuilder(this.ConnectionString);

                var databaseName = builder.InitialCatalog;
                builder.InitialCatalog = "master";

                var connectionString = builder.ToString();

                using var connection = new SqlConnection(connectionString);
                connection.Open();

                if (CheckDatabaseExists(databaseName, connection))
                {
                    SqlConnection.ClearAllPools();

                    using var commandAlter = new SqlCommand($"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", connection);
                    commandAlter.ExecuteNonQuery();

                    using var commandDrop = new SqlCommand($"DROP DATABASE [{databaseName}]", connection);
                    commandDrop.ExecuteNonQuery();
                }
            }
        }
    }
}
