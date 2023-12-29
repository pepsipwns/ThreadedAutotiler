using System.Collections.Generic;
using Godot;

public class DecorativeTileData
{
    public Vector2I AtlasCoords;
    public int Direction;

    public float Chance = 100;

    public DecorativeTileData(Vector2I atlasCoords, int direction, float chance = 100)
    {
        AtlasCoords = atlasCoords;
        Direction = direction;
        Chance = chance;
    }

    public void SetData(Vector2I atlasCoords, float chance = 100)
    {
        AtlasCoords = atlasCoords;
        Chance = chance;
    }

    public void GetData(out Vector2I atlasCoords, out int direction, out float chance)
    {
        atlasCoords = AtlasCoords;
        direction = Direction;
        chance = Chance;
    }
}
