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
        private readonly ITilesRepo _tilesRepo;
        public TilesController(ITilesRepo repo)
        {
            _tilesRepo = repo;
        }

        public class Tile2DItemList
        {
            public List<Tile2DItem> Tiles { get; set; }
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
