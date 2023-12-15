using System;
using Godot;

public class BitmaskTile
{
    public ushort[] BitmaskValue;

    public Vector2I AtlasValue;

    public BitmaskTile(Vector2I atlasValue, ushort[] bitmaskValue)
    {
        AtlasValue = atlasValue;
        BitmaskValue = bitmaskValue;
    }
}
