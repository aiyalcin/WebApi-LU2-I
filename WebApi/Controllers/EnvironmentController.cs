using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/environments")]
    public class EnvironmentController : Controller
    {
        private readonly string _connectionString;
        public EnvironmentController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
        }

        [HttpPost]
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
                    command.Parameters.AddWithValue("@MaxLength", environment.MaxLenght);
                    command.Parameters.AddWithValue("@Username", environment.Username);
                    command.ExecuteNonQuery();
                }
                connection1.Close();
            }
        }
    }
}
