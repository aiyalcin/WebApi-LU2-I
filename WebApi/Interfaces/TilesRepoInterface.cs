using WebApi.Items;

public interface ITilesRepo
{
    Task<List<Tile2DItem?>> ReadTilesAsync(string WorldId);
    Task SaveTiles(List<Tile2DItem> tiles);
}
