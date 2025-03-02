using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using WebApi.Items;

namespace WebApi.DataBase
{
    public class EnvironmentRepo
    {
        private readonly string _connectionString;
        private readonly ILogger<EnvironmentRepo> _logger;
        public EnvironmentRepo(IConfiguration configuration, ILogger<EnvironmentRepo> logger)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
        }
        public async Task SaveEnvironment(EnvironmentItem environment)
        {
            using (var connection1 = new SqlConnection(_connectionString))
            {
                connection1.Open();
                using (var command = connection1.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Environments2D (Id, Name, Height, Width, UserId) VALUES (@Id, @Name, @Height, @Length, @UserId)";
                    command.Parameters.AddWithValue("@Id", environment.Id);
                    command.Parameters.AddWithValue("@Name", environment.Name);
                    command.Parameters.AddWithValue("@Height", environment.Height);
                    command.Parameters.AddWithValue("@Width", environment.Width);
                    command.Parameters.AddWithValue("@UserId", environment.UserId);
                    command.ExecuteNonQuery();
                }
                connection1.Close();
            }
        }

        public async Task<List<EnvironmentItem?>> ReadEnvironmentsAsync(string userName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query1 = "SELECT Id, UserName FROM auth.AspNetUsers WHERE UserName = @userName";
                var user = await connection.QuerySingleOrDefaultAsync<UserItem>(query1, new { userName = userName });
                _logger.LogInformation($"User: {user.Id}");
                var query = "SELECT Id, Name, Height, Width AS Width FROM Environments2D WHERE UserId = @UserId";
                var environments = (await connection.QueryAsync<EnvironmentItem>(query, new { userId = user.Id })).ToList();
                return environments;
            }
        }
        public async Task<EnvironmentItem?> ReadEnvironmentAsync(string userName, string worldName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query1 = "SELECT Id, UserName FROM auth.AspNetUsers WHERE UserName = @userName";
                var user = await connection.QuerySingleOrDefaultAsync<UserItem>(query1, new { userName = userName });
                _logger.LogInformation($"User: {user.Id}");
                var query = $"SELECT Id, Name, Height, Width AS Width FROM Environments2D WHERE UserId = @UserId AND Name = @Name";
                var environment = await connection.QuerySingleOrDefaultAsync<EnvironmentItem>(query, new { userId = user.Id, Name = worldName });
                return environment;
            }
        }
    }
}
