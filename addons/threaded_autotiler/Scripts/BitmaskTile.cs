using System;
using Godot;

public class BitmaskTile
{
    public ushort BitmaskValue;

    public Vector2I AtlasValue;

    public double Chance;

    public BitmaskTile(Vector2I atlasValue, ushort bitmaskValue, double chance = 100)
    {
        AtlasValue = atlasValue;
        BitmaskValue = bitmaskValue;
        Chance = chance;
    }
}
