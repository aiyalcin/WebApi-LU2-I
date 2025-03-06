using System.Windows.Input;
using Microsoft.Data.SqlClient;
using WebApi.Items;

namespace WebApi.DataBase
{
    public class TilesRepo
    {
        private readonly string _connectionString;
        private List<Tile2DItem> WorldTiles;

        public TilesRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ??
                                throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
        }

        public async Task<List<Tile2DItem?>> ReadTilesAsync(string environmentId)
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
                        var tiles = new List<Tile2DItem>();
                        while (await reader.ReadAsync())
                        {
                            var tile = new Tile2DItem
                            {
                                Id = reader["Id"].ToString(),
                                TileName = reader["TileName"].ToString(),
                                PositionX = Convert.ToInt32(reader["PositionX"]),
                                PositionY = Convert.ToInt32(reader["PositionY"]),
                                EnvironmentName = reader["EnvironmentID"].ToString()
                            };
                            tiles.Add(tile);
                        }

                        return tiles;
                    }
                }
            }
        }

        public async Task SaveTile(Tile2DItem tile)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Tile2D (Id, TileName, PositionX, PositionY, EnvironmentName) VALUES (@Id, @TileName, @PositionX, @PositionY, @EnvironmentName)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", tile.Id);
                    command.Parameters.AddWithValue("@TileName", tile.TileName);
                    command.Parameters.AddWithValue("@PositionX", tile.PositionX);
                    command.Parameters.AddWithValue("@PositionY", tile.PositionY);
                    command.Parameters.AddWithValue("@EnvironmentName", tile.EnvironmentName);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
