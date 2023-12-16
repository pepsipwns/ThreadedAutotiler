using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TerrainHandler : Node
{
    public BitmaskTile[] Dirt()
    {
        return new BitmaskTile[]
        {
            //Center Tiles
            new BitmaskTile(new Vector2I(0, 15), 255),
            //Bottom
            new BitmaskTile(new Vector2I(8, 15), BitmaskStatic.Bottom),
            //Top
            new BitmaskTile(new Vector2I(8, 14), BitmaskStatic.Top),
            //Left
            new BitmaskTile(new Vector2I(10, 15), BitmaskStatic.Left),
            //Right
            new BitmaskTile(new Vector2I(11, 15), BitmaskStatic.Right),
            //Bottom Left
            new BitmaskTile(new Vector2I(3, 15), BitmaskStatic.BottomLeft),
            //Bottom Right
            new BitmaskTile(new Vector2I(4, 15), BitmaskStatic.BottomRight),
            //Top Left
            new BitmaskTile(new Vector2I(3, 14), BitmaskStatic.TopLeft),
            //Top Right
            new BitmaskTile(new Vector2I(4, 14), BitmaskStatic.TopRight),
            //Single Left Right
            new BitmaskTile(new Vector2I(15, 15), BitmaskStatic.SingleLeftRight),
            //Single Top Bottom
            new BitmaskTile(new Vector2I(12, 15), BitmaskStatic.SingleTopBottom),
            //Single Left
            new BitmaskTile(new Vector2I(14, 14), BitmaskStatic.SingleLeft),
            //Single Right
            new BitmaskTile(new Vector2I(15, 14), BitmaskStatic.SingleRight),
            //Single Top
            new BitmaskTile(new Vector2I(13, 14), BitmaskStatic.SingleTop),
            //Single Bottom
            new BitmaskTile(new Vector2I(13, 15), BitmaskStatic.SingleBottom),
        };
    }

    public BitmaskTile[] Grass()
    {
        return new BitmaskTile[]
        {
            //Center Tiles
            new BitmaskTile(new Vector2I(0, 23), 255),
            //Bottom
            new BitmaskTile(new Vector2I(6, 23), BitmaskStatic.Bottom),
            //Top
            new BitmaskTile(new Vector2I(6, 22), BitmaskStatic.Top),
            //Left
            new BitmaskTile(new Vector2I(10, 23), BitmaskStatic.Left),
            //Right
            new BitmaskTile(new Vector2I(9, 23), BitmaskStatic.Right),
            //Bottom Left
            new BitmaskTile(new Vector2I(2, 23), BitmaskStatic.BottomLeft),
            //Bottom Right
            new BitmaskTile(new Vector2I(3, 23), BitmaskStatic.BottomRight),
            //Top Left
            new BitmaskTile(new Vector2I(2, 22), BitmaskStatic.TopLeft),
            //Top Right
            new BitmaskTile(new Vector2I(3, 22), BitmaskStatic.TopRight),
        };
    }

    public static TerrainHandler Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public static TerrainType[] TerrainTypes =
        TerrainType.GetValues(typeof(TerrainType)) as TerrainType[];

    public static float GetTerrainHeight(TerrainType type)
    {
        switch (type)
        {
            case TerrainType.Dirt:
                return -.2f;
            case TerrainType.Grass:
                return -.05f;
            default:
                return 0.0f;
        }
    }

    public Vector2I GetAtlasTile(TerrainType type, ushort bitmask, int x, int y)
    {
        BitmaskTile[] tiles = GetBitmaskTiles(type);
        foreach (BitmaskTile tile in tiles)
        {
            if (tile.BitmaskValue == bitmask)
            {
                return tile.AtlasValue;
            }
        }
        if (bitmask == 0)
        {
            return new Vector2I(-1, -1);
        }

        return tiles[0].AtlasValue;
    }

    public BitmaskTile[] GetBitmaskTiles(TerrainType type)
    {
        switch (type)
        {
            case TerrainType.Dirt:
                return Dirt();
            case TerrainType.Grass:
                return Grass();
            default:
                return null;
        }
    }
}
