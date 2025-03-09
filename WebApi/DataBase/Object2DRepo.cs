//using Dapper;
//using Microsoft.Data.SqlClient;
//using WebApi.Items;

//namespace WebApi.DataBase
//{
//    public class Object2DRepo
//    {
//        private readonly string _connectionString;
//        public Object2DRepo(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("ConnectionString1") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");
//        }

//        public async Task SaveObject2D(List<Object2DItem> objects)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                connection.Open();
//                using (var transaction = connection.BeginTransaction())
//                {
//                    try
//                    {
//                        foreach (var object2d in objects)
//                        {
//                            var query = "INSERT INTO Object2D (Id, PositionX, PositionY, ScaleX, ScaleY, Rotation, FloatingLayer, WorldId) VALUES (@Id, @PositionX, @PositionY, @ScaleX, @ScaleY, @Rotation, @FloatingLayer, @WorldId)";
//                            await connection.ExecuteAsync(query, new
//                            {
//                                object2d.Id,
//                                object2d.PositionX,
//                                object2d.PositionY,
//                                object2d.ScaleX,
//                                object2d.ScaleY,
//                                object2d.Rotation,
//                                object2d.FloatingLayer,
//                                object2d.WorldId
//                            }, transaction);
//                        }
//                        transaction.Commit();
//                    }
//                    catch
//                    {
//                        transaction.Rollback();
//                        throw;
//                    }
//                }
//                connection.Close();
//            }
//        }

//        public async Task SaveItem(PrefabItem item)
//        {
//            using (var connection1 = new SqlConnection(_connectionString))
//            {
//                connection1.Open();
//                using (var command = connection1.CreateCommand())
//                {
//                    command.CommandText = "INSERT INTO Object2D (Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, WorldId) VALUES (@Id, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @WorldId)";
//                    command.Parameters.AddWithValue("@Id", item.Id);
//                    command.Parameters.AddWithValue("@PrefabId", item.PrefabId);
//                    command.Parameters.AddWithValue("@PositionX", item.PositionX);
//                    command.Parameters.AddWithValue("@PositionY", item.PositionY);
//                    command.Parameters.AddWithValue("@ScaleX", item.ScaleX);
//                    command.Parameters.AddWithValue("@ScaleY", item.ScaleY);
//                    command.Parameters.AddWithValue("@RotationZ", item.Rotation);
//                    command.Parameters.AddWithValue("@SortingLayer", item.FloatingLayer);
//                    command.Parameters.AddWithValue("@WorldId", item.WorldId);
//                    command.ExecuteNonQuery();
//                }
//                connection1.Close();
//            }
//        }

//        public async Task<PrefabItem?> ReadItemsAsnyc(string Id)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                var query = "SELECT Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ AS Rotation, SortingLayer AS FloatingLayer, EnvironmentID FROM Object2D WHERE Id = @Id";
//                var item = await connection.QuerySingleOrDefaultAsync<PrefabItem>(query, new { Id = Id });
//                return item;
//            }
//        }


//    }
//}
