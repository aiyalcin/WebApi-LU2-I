using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/users")]
    public class UserController : Controller
    {
        private readonly string _connectionString;
        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
        }
        [HttpPost]
        public async Task SaveUser(UserItem user)
        {
            using (var connection1 = new SqlConnection(_connectionString))
            {
                connection1.Open();
                using (var command = connection1.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.ExecuteNonQuery();
                }
                connection1.Close();
            }
        }
    }
}
