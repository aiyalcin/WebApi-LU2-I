using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApi.DataBase;
using WebApi.Items;

namespace WebApi.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("/users")]
    public class UserController : Controller
    {
        private readonly string _connectionString;
        UserRepo userRepo;
        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString1' not found.");
            userRepo = new UserRepo(configuration);
        }
        [HttpPost]
        public async Task SaveUser(UserItem user)
        {
            await userRepo.SaveUser(user);
            Ok();
        }

        [HttpDelete]
        public async Task DeleteUser(string username)
        {
            await userRepo.DeleteUser(username);
            Ok();
        }
    }
}
