using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataBase;
using WebApi.Items;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/tiles")]
    public class TilesController : Controller
    {
        private readonly string _connectionString;
        private readonly EnvironmentRepo _environmentRepo;
        private readonly TilesRepo _tilesRepo;
        private readonly ILogger<EnvironmentRepo> _logger;

        public class Tile2DItemList
        {
            public List<Tile2DItem> Tiles { get; set; }
        }

        public TilesController(IConfiguration configuration, ILogger<EnvironmentRepo> logger)
        {
            _connectionString = configuration.GetValue<string>("ConnectionString1")
                                ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
            _environmentRepo = new EnvironmentRepo(configuration, logger);
            _tilesRepo = new TilesRepo(configuration);
        }

        //get all tiles in an environment
        [HttpGet("{WorldId}")]
        public async Task<List<Tile2DItem?>> ReadTilesAsync(string WorldId)
        {
            return await _tilesRepo.ReadTilesAsync(WorldId);
        }

        [HttpPost]
        public async Task SaveTiles([FromBody] Tile2DItemList tileList)
        {
            await _tilesRepo.SaveTiles(tileList.Tiles);
        }
    }
}
