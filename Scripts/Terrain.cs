public class Terrain
{
    public string Name;
    public BitmaskTile[] Tiles;

    public Terrain(string name, BitmaskTile[] tiles)
    {
        Name = name;
        Tiles = tiles;
    }
}
