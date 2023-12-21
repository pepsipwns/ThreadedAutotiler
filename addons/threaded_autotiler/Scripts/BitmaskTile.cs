using System;
using Godot;

public class BitmaskTile
{
    public ushort BitmaskValue;

    public Vector2I AtlasValue;

    public float Chance;

    public BitmaskTile(Vector2I atlasValue, ushort bitmaskValue, float chance = 100)
    {
        AtlasValue = atlasValue;
        BitmaskValue = bitmaskValue;
        Chance = chance;
    }
}
