using Microsoft.AspNetCore.Mvc;
using WebApi.DataBase;
using WebApi.Items;

namespace WebApi.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("/tiles")]
    public class TilesController : Controller
    {
        private readonly string _connectionString;
        private readonly EnvironmentRepo _environmentRepo;
        private readonly TilesRepo _tilesRepo;
        private readonly ILogger<EnvironmentRepo> _logger;
        public TilesController(IConfiguration configuration, ILogger<EnvironmentRepo> logger)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1")
                                ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
            _environmentRepo = new EnvironmentRepo(configuration, logger);
        }

        //get all tiles in an environment
        [HttpGet("{EnvironmentId}")]
        public async Task<List<Tile2D?>> ReadTilesAsync(string EnvironmentId)
        {
            return await _tilesRepo.ReadTilesAsync(EnvironmentId);
        }
    }
}
