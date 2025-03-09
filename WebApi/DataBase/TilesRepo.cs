using System.Windows.Input;
using Dapper;
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

        public async Task<List<Tile2DItem?>> ReadTilesAsync(string WorldId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Tile2D WHERE WorldId = @WorldId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WorldId", WorldId);

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
                                WorldId = reader["WorldId"].ToString()
                            };
                            tiles.Add(tile);
                        }
                        return tiles;
                    }
                }
            }
        }

        public async Task SaveTiles(List<Tile2DItem> tiles)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var tile in tiles)
                        {
                            var query = "INSERT INTO Tile2D (Id, TileName, PositionX, PositionY, WorldId) VALUES (@Id, @TileName, @PositionX, @PositionY, @WorldId)";
                            await connection.ExecuteAsync(query, new
                            {
                                tile.Id,
                                tile.TileName,
                                tile.PositionX,
                                tile.PositionY,
                                tile.WorldId
                            }, transaction);
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                connection.Close();
            }
        }



    }
}
