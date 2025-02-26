using Microsoft.Data.SqlClient;
using WebApi.Items;

namespace WebApi.DataBase
{
    public class UserRepo
    {
        private string _connectionString;
        public UserRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
        }

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

        public async Task DeleteUser(string username)
        {
            using (var connection1 = new SqlConnection(_connectionString))
            {
                connection1.Open();
                using (var command = connection1.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Users WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();
                }
                connection1.Close();
            }
        }
    }
}
