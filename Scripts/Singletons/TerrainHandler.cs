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
            new BitmaskTile(new Vector2I(0, 15), new ushort[] { 255 }),
            //Bottom
            new BitmaskTile(new Vector2I(8, 15), Bottom),
            //Top
            new BitmaskTile(new Vector2I(8, 14), Top),
            //Left
            new BitmaskTile(new Vector2I(10, 15), Left),
            //Right
            new BitmaskTile(new Vector2I(11, 15), Right),
            //Bottom Left
            new BitmaskTile(new Vector2I(3, 15), BottomLeft),
            //Bottom Right
            new BitmaskTile(new Vector2I(4, 15), BottomRight),
            //Top Left
            new BitmaskTile(new Vector2I(3, 14), TopLeft),
            //Top Right
            new BitmaskTile(new Vector2I(4, 14), TopRight),
            //Single Left Right
            new BitmaskTile(new Vector2I(15, 15), SingleLeftRight),
            //Single Top Bottom
            new BitmaskTile(new Vector2I(12, 15), SingleTopBottom),
            //Single Left
            new BitmaskTile(new Vector2I(14, 14), SingleLeft),
            //Single Right
            new BitmaskTile(new Vector2I(15, 14), SingleRight),
            //Single Top
            new BitmaskTile(new Vector2I(13, 14), SingleTop),
            //Single Bottom
            new BitmaskTile(new Vector2I(13, 15), SingleBottom),
        };
    }

    public BitmaskTile[] Grass()
    {
        return new BitmaskTile[]
        {
            //Center Tiles
            new BitmaskTile(new Vector2I(0, 23), new ushort[] { 255 }),
            //Bottom
            new BitmaskTile(new Vector2I(6, 23), Bottom),
            //Top
            new BitmaskTile(new Vector2I(6, 22), Top),
            //Left
            new BitmaskTile(new Vector2I(10, 23), Left),
            //Right
            new BitmaskTile(new Vector2I(9, 23), Right),
            //Bottom Left
            new BitmaskTile(new Vector2I(2, 23), BottomLeft),
            //Bottom Right
            new BitmaskTile(new Vector2I(3, 23), BottomRight),
            //Top Left
            new BitmaskTile(new Vector2I(2, 22), TopLeft),
            //Top Right
            new BitmaskTile(new Vector2I(3, 22), TopRight),
        };
    }

    private ushort[] Top = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(t: false, tr: false, tl: false),
    };

    private ushort[] Bottom = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(b: false, br: false, bl: false),
    };

    private ushort[] Left = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(l: false, tl: false, bl: false),
    };

    private ushort[] Right = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(r: false, tr: false, br: false),
    };

    private ushort[] TopRight = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(r: false, tr: false, t: false),
    };

    private ushort[] TopLeft = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(t: false, tl: false, l: false),
    };

    private ushort[] BottomRight = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(b: false, br: false, r: false),
    };

    private ushort[] BottomLeft = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(b: false, bl: false, l: false),
    };

    private ushort[] SingleLeftRight = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(t: false, b: false),
    };

    private ushort[] SingleTopBottom = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(l: false, r: false),
    };

    private ushort[] SingleLeft = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(l: false, b: false, t: false),
    };

    private ushort[] SingleRight = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(r: false, b: false, t: false),
    };

    private ushort[] SingleTop = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(l: false, r: false, t: false),
    };

    private ushort[] SingleBottom = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(l: false, r: false, b: false),
    };

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
            if (tile.BitmaskValue.Contains(bitmask))
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
