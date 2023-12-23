using Godot;

public class TileData
{
    public int Id;
    public Vector2I AtlasCoords;
    public string TileMode;

    public bool[] TileBitmasks = new bool[9];

    public float Chance = 100;

    public TileData(
        int id,
        Vector2I atlasCoords,
        string tileMode,
        bool[] bitmasks,
        float chance = 100
    )
    {
        Id = id;
        AtlasCoords = atlasCoords;
        TileMode = tileMode;
        TileBitmasks = bitmasks;
        Chance = chance;
    }

    public void SetData(
        int id,
        Vector2I atlasCoords,
        string tileMode,
        bool[] bitmasks,
        float chance = 100
    )
    {
        Id = id;
        AtlasCoords = atlasCoords;
        TileMode = tileMode;
        TileBitmasks = bitmasks;
        Chance = chance;
    }

    public void GetData(
        out int id,
        out Vector2I atlasCoords,
        out string tileMode,
        out bool[] bitmasks,
        out float chance
    )
    {
        id = Id;
        atlasCoords = AtlasCoords;
        tileMode = TileMode;
        bitmasks = TileBitmasks;
        chance = Chance;
    }
}
