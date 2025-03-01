using Dapper;
using Microsoft.Data.SqlClient;
using WebApi.Items;

namespace WebApi.DataBase
{
    public class EnvironmentRepo
    {
        private readonly string _connectionString;
        public EnvironmentRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
        }
        public async Task SaveEnvironment(EnvironmentItem environment)
        {
            using (var connection1 = new SqlConnection(_connectionString))
            {
                connection1.Open();
                using (var command = connection1.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Environments2D (Id, Name, MaxHeight, MaxLength, Username) VALUES (@Id, @Name, @MaxHeight, @MaxLength, @Username)";
                    command.Parameters.AddWithValue("@Id", environment.Id);
                    command.Parameters.AddWithValue("@Name", environment.Name);
                    command.Parameters.AddWithValue("@MaxHeight", environment.MaxHeight);
                    command.Parameters.AddWithValue("@MaxLength", environment.MaxLength);
                    command.Parameters.AddWithValue("@Username", environment.Username);
                    command.ExecuteNonQuery();
                }
                connection1.Close();
            }
        }

        public async Task<EnvironmentItem?> ReadEnvironmentAsync(string Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, MaxHeight, MaxLength AS MaxLength, Username FROM Environments2D WHERE Id = @Id";
                var environment = await connection.QuerySingleOrDefaultAsync<EnvironmentItem>(query, new { Id = Id });
                return environment;
            }
        }
        public async Task<IEnumerable<EnvironmentItem>> ReadAllEnvironmentsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, MaxHeight, MaxLength AS MaxLength, Username FROM Environments2D";
                var environments = await connection.QueryAsync<EnvironmentItem>(query);
                return environments;
            }
        }
    }
}
