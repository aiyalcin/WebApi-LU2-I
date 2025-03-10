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
        private readonly ILogger<EnvironmentRepo> _logger;
        public EnvironmentController(IConfiguration configuration, ILogger<EnvironmentRepo> logger)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1")
                                ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
            _environmentRepo = new EnvironmentRepo(configuration, logger);
        }

        [HttpPost]
        public async Task SaveEnvironment(EnvironmentItem environment)
        {
            await _environmentRepo.SaveEnvironment(environment);
        }

        [HttpGet("{email}")]
        public async Task<List<EnvironmentItem?>> ReadEnvironmentsAsync(string email)
        {
            return await _environmentRepo.ReadEnvironmentsAsync(email);
        }

        [HttpGet("{email}/world/{worldName}")]
        public async Task<EnvironmentItem?> ReadEnvironmentAsync(string email, string worldName)
        {
            return await _environmentRepo.ReadEnvironmentAsync(email, worldName);
        }

        [HttpDelete("{WorldId}")]
        public async Task DeleteEnvironment(string WorldId)
        {
            await _environmentRepo.DeleteEnvironment(WorldId);
        }

        [HttpGet("{email}/world/{worldName}/exists")]
        public async Task<bool> CheckExits(string email, string worldName)
        {
            return await _environmentRepo.CheckExits(email, worldName);
        }
    }
}