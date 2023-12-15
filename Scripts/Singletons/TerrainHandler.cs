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
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, bl: true, b: true, br: true),
        BitmaskConverter.ConvertTileToBitmask(
            l: true,
            r: true,
            bl: true,
            b: true,
            br: true,
            tr: true
        ),
        BitmaskConverter.ConvertTileToBitmask(
            l: true,
            r: true,
            bl: true,
            b: true,
            br: true,
            tl: true
        ),
        BitmaskConverter.ConvertTileToBitmask(
            l: true,
            r: true,
            bl: true,
            b: true,
            br: true,
            tl: true,
            tr: true
        ),
    };

    private ushort[] Bottom = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, tl: true, t: true, tr: true),
        BitmaskConverter.ConvertTileToBitmask(
            l: true,
            r: true,
            tl: true,
            t: true,
            tr: true,
            br: true
        ),
        BitmaskConverter.ConvertTileToBitmask(
            l: true,
            r: true,
            tl: true,
            t: true,
            tr: true,
            bl: true
        ),
        BitmaskConverter.ConvertTileToBitmask(
            l: true,
            r: true,
            tl: true,
            t: true,
            tr: true,
            bl: true,
            br: true
        ),
    };

    private ushort[] Left = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(t: true, b: true, r: true, tr: true, br: true),
        BitmaskConverter.ConvertTileToBitmask(
            t: true,
            b: true,
            r: true,
            tr: true,
            br: true,
            bl: true
        ),
        BitmaskConverter.ConvertTileToBitmask(
            t: true,
            b: true,
            r: true,
            tr: true,
            br: true,
            tl: true
        ),
        BitmaskConverter.ConvertTileToBitmask(
            t: true,
            b: true,
            r: true,
            tr: true,
            br: true,
            tl: true,
            bl: true
        ),
    };

    private ushort[] Right = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(t: true, b: true, l: true, tl: true, bl: true),
        BitmaskConverter.ConvertTileToBitmask(
            t: true,
            b: true,
            l: true,
            tl: true,
            bl: true,
            br: true
        ),
        BitmaskConverter.ConvertTileToBitmask(
            t: true,
            b: true,
            l: true,
            tl: true,
            bl: true,
            tr: true
        ),
        BitmaskConverter.ConvertTileToBitmask(
            t: true,
            b: true,
            l: true,
            tl: true,
            bl: true,
            tr: true,
            br: true
        ),
    };

    private ushort[] TopRight = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(l: true, bl: true, b: true),
        BitmaskConverter.ConvertTileToBitmask(l: true, bl: true, b: true, br: true),
        BitmaskConverter.ConvertTileToBitmask(tl: true, l: true, bl: true, b: true),
        BitmaskConverter.ConvertTileToBitmask(tl: true, l: true, bl: true, b: true, br: true),
    };

    private ushort[] TopLeft = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(r: true, br: true, b: true),
        BitmaskConverter.ConvertTileToBitmask(r: true, br: true, b: true, bl: true),
        BitmaskConverter.ConvertTileToBitmask(tr: true, r: true, br: true, b: true),
        BitmaskConverter.ConvertTileToBitmask(tr: true, r: true, br: true, b: true, bl: true),
    };

    private ushort[] BottomRight = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(t: true, tl: true, l: true),
        BitmaskConverter.ConvertTileToBitmask(t: true, tl: true, l: true, bl: true),
        BitmaskConverter.ConvertTileToBitmask(t: true, tl: true, l: true, tr: true),
        BitmaskConverter.ConvertTileToBitmask(t: true, tl: true, l: true, bl: true, tr: true),
    };

    private ushort[] BottomLeft = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(t: true, tr: true, r: true),
        BitmaskConverter.ConvertTileToBitmask(t: true, tr: true, r: true, br: true),
        BitmaskConverter.ConvertTileToBitmask(t: true, tr: true, r: true, tl: true),
        BitmaskConverter.ConvertTileToBitmask(t: true, tr: true, r: true, br: true, tl: true),
    };

    private ushort[] SingleLeftRight = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true),
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, bl: true),
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, br: true),
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, tr: true),
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, tl: true),
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, bl: true, br: true),
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, bl: true, tr: true),
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, bl: true, tl: true),
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, bl: true, br: true, tl: true),
        BitmaskConverter.ConvertTileToBitmask(l: true, r: true, bl: true, br: true, tr: true),
        BitmaskConverter.ConvertTileToBitmask(
            l: true,
            r: true,
            bl: true,
            br: true,
            tl: true,
            tr: true
        ),
    };

    private ushort[] SingleTopBottom = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(t: true, b: true),
        BitmaskConverter.ConvertTileToBitmask(t: true, b: true, tl: true),
        BitmaskConverter.ConvertTileToBitmask(t: true, b: true, tl: true, tr: true),
        BitmaskConverter.ConvertTileToBitmask(t: true, b: true, tl: true, bl: true),
    };

    private ushort[] SingleLeft = new ushort[]
    {
        BitmaskConverter.ConvertTileToBitmask(r: true, b: true),
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
