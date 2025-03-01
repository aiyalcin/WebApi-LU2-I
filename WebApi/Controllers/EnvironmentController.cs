using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApi.DataBase;
using WebApi.Items;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/environments")]
    public class EnvironmentController : Controller
    {
        private readonly string _connectionString;
        private readonly EnvironmentRepo _environmentRepo;
        public EnvironmentController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
            _environmentRepo = new EnvironmentRepo(configuration);
        }

        [HttpPost]
        public async Task SaveEnvironment(EnvironmentItem environment)
        {
            _environmentRepo.SaveEnvironment(environment);
        }

        [HttpGet("{id}")]
        public async Task<EnvironmentItem?> ReadEnvironmentAsync(string id)
        {
            return await _environmentRepo.ReadEnvironmentAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<EnvironmentItem>> ReadAllEnvironmentsAsync()
        {
            return await _environmentRepo.ReadAllEnvironmentsAsync();
        }

    }
}
