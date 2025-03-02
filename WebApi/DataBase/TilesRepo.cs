using System.Windows.Input;
using Microsoft.Data.SqlClient;
using WebApi.Items;

namespace WebApi.DataBase
{
    public class TilesRepo
    {
        private readonly string _connectionString;
        private List<Tile2D> WorldTiles;

        public TilesRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ??
                                throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
        }

        public async Task<List<Tile2D?>> ReadTilesAsync(string environmentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Tiles WHERE EnvironmentId = @EnvironmentId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EnvironmentId", environmentId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var tiles = new List<Tile2D>();
                        while (await reader.ReadAsync())
                        {
                            var tile = new Tile2D
                            {
                                Id = reader["Id"].ToString(),
                                TileName = reader["TileName"].ToString(),
                                PositionX = Convert.ToInt32(reader["PositionX"]),
                                PositionY = Convert.ToInt32(reader["PositionY"]),
                                EnvironmentID = reader["EnvironmentID"].ToString()
                            };
                            tiles.Add(tile);
                        }

                        return tiles;
                    }
                }
            }
        }

    }
}
