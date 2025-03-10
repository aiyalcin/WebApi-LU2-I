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
                var query1 = "SELECT Id FROM auth.AspNetUsers WHERE UserName = @userName";
                var userId = await connection1.QuerySingleOrDefaultAsync<string>(query1, new { userName = environment.Username });
                using (var command = connection1.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Environments2D (WorldName, Username, Height, Width, UserId, WorldId) VALUES (@WorldName, @Username, @Height, @Width, @UserId, @WorldId)";
                    command.Parameters.AddWithValue("@Username", environment.Username);
                    command.Parameters.AddWithValue("@Height", environment.Height);
                    command.Parameters.AddWithValue("@Width", environment.Width);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("WorldName", environment.WorldName);
                    command.Parameters.AddWithValue("WorldId", environment.WorldId);
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
                var query = "SELECT WorldName, UserName, Height, Width AS Width, WorldId FROM Environments2D WHERE UserId = @UserId";
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
                var query = "SELECT WorldId, WorldName, UserName, Height, Width AS Width FROM Environments2D WHERE UserId = @UserId AND WorldName = @WorldName";
                var environment = await connection.QuerySingleOrDefaultAsync<EnvironmentItem>(query, new { userId = user.Id, WorldName = worldName });
                return environment;
            }
        }
        public async Task DeleteEnvironment(string WorldId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Environments2D WHERE WorldId = @WorldId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WorldId", WorldId);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public async Task<bool> CheckExits(string userName, string worldName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query1 = "SELECT Id, UserName FROM auth.AspNetUsers WHERE UserName = @userName";
                var user = await connection.QuerySingleOrDefaultAsync<UserItem>(query1, new { userName = userName });
                var query = "SELECT WorldId, WorldName, UserName, Height, Width AS Width FROM Environments2D WHERE UserId = @UserId AND WorldName = @WorldName";
                var environment = await connection.QuerySingleOrDefaultAsync<EnvironmentItem>(query, new { userId = user.Id, WorldName = worldName });
                return environment != null;
            }
        }

    }
}
