using System.Collections.Generic;
using Godot;

public class TileData
{
    public int Id;
    public Vector2I AtlasCoords;
    public string TileMode;

    public bool[] TileBitmasks = new bool[9];

    public float Chance = 100;

    public List<DecorativeTileData> DecorativeTiles = new List<DecorativeTileData>();

    public TileData(
        int id,
        Vector2I atlasCoords,
        string tileMode,
        bool[] bitmasks,
        float chance = 100,
        List<DecorativeTileData> decorativeTiles = null
    )
    {
        Id = id;
        AtlasCoords = atlasCoords;
        TileMode = tileMode;
        TileBitmasks = bitmasks;
        Chance = chance;
        if (decorativeTiles != null)
        {
            DecorativeTiles = decorativeTiles;
        }
    }

    public void SetData(
        int id,
        Vector2I atlasCoords,
        string tileMode,
        bool[] bitmasks,
        List<DecorativeTileData> decorativeTiles,
        float chance = 100
    )
    {
        Id = id;
        AtlasCoords = atlasCoords;
        TileMode = tileMode;
        TileBitmasks = bitmasks;
        DecorativeTiles = decorativeTiles;
        Chance = chance;
    }

    public void SetData(TileData td)
    {
        Id = td.Id;
        AtlasCoords = td.AtlasCoords;
        TileMode = td.TileMode;
        TileBitmasks = td.TileBitmasks;
        Chance = td.Chance;
    }

    public void GetData(
        out int id,
        out Vector2I atlasCoords,
        out string tileMode,
        out bool[] bitmasks,
        out List<DecorativeTileData> decorativeTiles,
        out float chance
    )
    {
        id = Id;
        atlasCoords = AtlasCoords;
        tileMode = TileMode;
        bitmasks = TileBitmasks;
        decorativeTiles = DecorativeTiles;
        chance = Chance;
    }
}
