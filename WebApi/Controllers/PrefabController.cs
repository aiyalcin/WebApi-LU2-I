using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using WebApi.DataBase;
using WebApi.Items;

namespace WebApi.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("/prefabs")]
    public class PrefabController : Controller
    {
        private readonly string _connectionString;
        private readonly PrefabRepo _prefabRepo;
        public PrefabController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
            _prefabRepo = new PrefabRepo(configuration);
        }
        [HttpPost]
        public async Task SaveItem(PrefabItem item)
        {
            await _prefabRepo.SaveItem(item);
        }

        [HttpGet("{Id}")]
        public async Task<PrefabItem?> ReadItemsAsnyc(string Id)
        {
            return await _prefabRepo.ReadItemsAsnyc(Id);
        }
    }
}
