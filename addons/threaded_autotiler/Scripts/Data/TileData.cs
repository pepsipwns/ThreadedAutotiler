using Godot;

public class TileData
{
    public int Id;
    public Vector2I AtlasCoords;
    public string TileMode;

    public TileData(int id, Vector2I atlasCoords, string tileMode)
    {
        Id = id;
        AtlasCoords = atlasCoords;
        TileMode = tileMode;
    }

    public void SetData(int id, Vector2I atlasCoords, string tileMode)
    {
        Id = id;
        AtlasCoords = atlasCoords;
        TileMode = tileMode;
    }

    public void GetData(out int id, out Vector2I atlasCoords, out string tileMode)
    {
        id = Id;
        atlasCoords = AtlasCoords;
        tileMode = TileMode;
    }
}
