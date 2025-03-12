using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using WebApi.DataBase;
using WebApi.Items;
using static WebApi.Controllers.TilesController;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/objects2d")]
    public class Object2DController : Controller
    {
        private readonly IObject2DRepo _object2DRepo;
        public Object2DController(IObject2DRepo repo)
        {
            _object2DRepo = repo;
        }
        public class Object2DItemList
        {
            public List<Object2DItem> objects2D { get; set; }
        }
        [HttpPost]
        public async Task SaveObject2D([FromBody] Object2DItemList objects2D)
        {
            await _object2DRepo.SaveObject2D(objects2D.objects2D);
        }

        [HttpGet("{WorldId}")]
        public async Task<List<Object2DItem?>> ReadObject2DAsync(string WorldId)
        {
            return await _object2DRepo.ReadObjectsAsnyc(WorldId);
        }

    }
}
