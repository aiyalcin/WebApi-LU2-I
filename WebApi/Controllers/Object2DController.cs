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
    [Route("/prefabs")]
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
        //[HttpPost]
        //public async Task SaveItem([FromBody] _object2DItems tileList item)
        //{
        //    await _object2DRepo.SaveItem(item);
        //}

    }
}
