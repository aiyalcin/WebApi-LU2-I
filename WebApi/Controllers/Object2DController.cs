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
    //[Authorize] DEBUG
    [Route("/objects2d")]
    public class Object2DController : Controller
    {
        private readonly string _connectionString;
        private readonly Object2DRepo _object2DRepo;
        private List<Object2DItem> _object2DItems;
        public Object2DController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
            _object2DRepo = new Object2DRepo(configuration);
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
