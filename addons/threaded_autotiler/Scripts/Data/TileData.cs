using Godot;

public class TileData
{
    public int Id;
    public Vector2I AtlasCoords;
    public string TileMode;

    public float Chance = 100;

    public TileData(int id, Vector2I atlasCoords, string tileMode, float chance = 100)
    {
        Id = id;
        AtlasCoords = atlasCoords;
        TileMode = tileMode;
        Chance = chance;
    }

    public void SetData(int id, Vector2I atlasCoords, string tileMode, float chance = 100)
    {
        Id = id;
        AtlasCoords = atlasCoords;
        TileMode = tileMode;
        Chance = chance;
    }

    public void GetData(out int id, out Vector2I atlasCoords, out string tileMode, out float chance)
    {
        id = Id;
        atlasCoords = AtlasCoords;
        tileMode = TileMode;
        chance = Chance;
    }
}
