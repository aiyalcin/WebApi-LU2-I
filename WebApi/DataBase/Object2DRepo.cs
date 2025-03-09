using Dapper;
using Microsoft.Data.SqlClient;
using WebApi.Items;

namespace WebApi.DataBase
{
    public class Object2DRepo
    {
        private readonly string _connectionString;
        public Object2DRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
        }

        public async Task SaveObject2D(List<Object2DItem> objects)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var object2d in objects)
                        {
                            var query = "INSERT INTO Object2D (Id, PositionX, PositionY, ScaleX, ScaleY, RotationZ, WorldId, Object2DName) VALUES (@Id, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @WorldId, @Object2DName)";
                            await connection.ExecuteAsync(query, new
                            {
                                object2d.Id,
                                object2d.PositionX,
                                object2d.PositionY,
                                object2d.ScaleX,
                                object2d.ScaleY,
                                object2d.RotationZ,
                                object2d.WorldId,
                                object2d.Object2DName
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

        public async Task<List<Object2DItem?>> ReadObjectsAsnyc(string WorldId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Object2D WHERE WorldId = @WorldId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WorldId", WorldId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var objects2D = new List<Object2DItem>();
                        while (await reader.ReadAsync())
                        {
                            var object2D = new Object2DItem()
                            {
                                Id = reader["Id"].ToString(),
                                Object2DName = reader["Object2DName"].ToString(),
                                PositionX = Convert.ToInt32(reader["PositionX"]),
                                PositionY = Convert.ToInt32(reader["PositionY"]),
                                ScaleX = Convert.ToInt32(reader["ScaleX"]),
                                ScaleY = Convert.ToInt32(reader["ScaleY"]),
                                RotationZ = Convert.ToInt32(reader["RotationZ"]),
                                WorldId = reader["WorldId"].ToString()
                            };
                            objects2D.Add(object2D);
                        }
                        return objects2D;
                    }
                }
            }
        }


    }
}
