using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataBase;
using WebApi.Items;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/environments")]
    public class EnvironmentController : Controller
    {
        private readonly IEnvironmentRepo _environmentRepo;
        private readonly ILogger<EnvironmentController> _logger;

        public EnvironmentController(IEnvironmentRepo repo, ILogger<EnvironmentController> logger)
        {
            _environmentRepo = repo;
            _logger = logger;
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
            _logger.LogInformation("CheckExits called with email: {Email}, worldName: {WorldName}", email, worldName);
            return await _environmentRepo.CheckExits(email, worldName);
        }
    }
}