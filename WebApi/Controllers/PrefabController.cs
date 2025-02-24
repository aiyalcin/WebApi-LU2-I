using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/prefabs")]
    public class PrefabController : Controller
    {
        private readonly string _connectionString;
        public PrefabController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
        }
        [HttpPost]
        public async Task SaveItem(PrefabItem item)
        {
            using (var connection1 = new SqlConnection(_connectionString))
            {
                connection1.Open();
                using (var command = connection1.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Object2D (Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, EnvironmentID) VALUES (@Id, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @EnvironmentID)";
                    command.Parameters.AddWithValue("@Id", item.Id);
                    command.Parameters.AddWithValue("@PrefabId", item.PrefabId);
                    command.Parameters.AddWithValue("@PositionX", item.PositionX);
                    command.Parameters.AddWithValue("@PositionY", item.PositionY);
                    command.Parameters.AddWithValue("@ScaleX", item.ScaleX);
                    command.Parameters.AddWithValue("@ScaleY", item.ScaleY);
                    command.Parameters.AddWithValue("@RotationZ", item.Rotation);
                    command.Parameters.AddWithValue("@SortingLayer", item.FloatingLayer);
                    command.Parameters.AddWithValue("@EnvironmentID", item.EnvironmentID);
                    command.ExecuteNonQuery();
                }
                connection1.Close();
            }
        }

        [HttpGet("{Id}")]
        public async Task<PrefabItem?> ReadItemsAsnyc(int Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync("SELECT * FROM PrefabItems WHERE Id = @Id", new { Id = Id });
            }
        }
    }
}
