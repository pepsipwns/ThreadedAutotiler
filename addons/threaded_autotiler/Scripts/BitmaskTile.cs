using System;
using System.Collections.Generic;
using Godot;

public class BitmaskTile
{
    public ushort BitmaskValue;

    public Vector2I AtlasValue;

    public float Chance;

    public List<DirectionBitmaskTile> DirectionBitmaskTiles = new List<DirectionBitmaskTile>();

    public BitmaskTile(
        Vector2I atlasValue,
        ushort bitmaskValue,
        float chance = 100,
        List<DirectionBitmaskTile> directionBitmaskTiles = null
    )
    {
        AtlasValue = atlasValue;
        BitmaskValue = bitmaskValue;
        Chance = chance;
        if (directionBitmaskTiles != null)
        {
            DirectionBitmaskTiles = directionBitmaskTiles;
        }
    }
}

public class DirectionBitmaskTile
{
    public Vector2I AtlasValue;

    public int Direction;

    public float Chance;

    public DirectionBitmaskTile(Vector2I atlasValue, int direction, float chance = 100)
    {
        AtlasValue = atlasValue;
        Direction = direction;
        Chance = chance;
    }
}
