using System;
using System.Collections.Generic;
using Godot;

/// <summary>
/// This class is responsible for generating the map data on a separate thread.
/// Firstly we create the terrain map, which is a 2D array of booleans representing whether a tile is there or not.
/// Then we create the bitmask map, which is a 2D array of bitmasks representing the value of the surrounding tiles, this is the data that will be saved to the file.
/// Then we create the atlas map, which is a 2D array of Vector2I representing the atlas tile to use for each tile, this is what will be used to create the actual TileMap.
///
/// </summary>
public partial class MapGeneration : Node
{
    public static MapGeneration Instance { get; private set; }
    public bool MapGenerated = false;
    public int Stage { get; private set; }
    public float StageProgress { get; private set; }
    public bool[][,] TerrainMap;
    public ushort[][,] BitmaskMap;
    public Vector2I[][,] TileAtlasMap;
    private int mapSize = 10;
    private bool useEdges = false;

    private Dictionary<string, List<TileData>> _tileData = new Dictionary<string, List<TileData>>();
    private Dictionary<string, BitmaskTile[]> _bitmaskTiles =
        new Dictionary<string, BitmaskTile[]>();

    private List<TerrainData> _terrainData = new List<TerrainData>();

    public override void _Ready()
    {
        Instance = this;
    }

    /// <summary>
    /// This generates all the map data based on the terrains and tiles you have created.
    /// This will need to be run first to generate the data that will be saved to file, and the atlas data that will be used to create the TileMap.
    /// </summary>
    /// <param name="noise">The FastNoiseLite noise that generates the map</param>
    /// <param name="mapSize">The size of the map (i.e. 100x100 tiles square)</param>
    /// <param name="useEdges">If the terrain generation rules should consider the edges as the same tile (joining) or not, useful for chunking.</param>
    public void GenerateMap(FastNoiseLite noise, int mapSize, bool useEdges)
    {
        this.mapSize = mapSize;
        this.useEdges = useEdges;
        double time = Time.GetUnixTimeFromSystem();
        LoadTerrainData();
        GenerateTerrainMap(noise);
        GenerateBitmaskMap();
        GenerateAtlasMap();
        MapGenerated = true;
        GD.Print(
            "[Threaded Autotiler] Map generated in "
                + (Time.GetUnixTimeFromSystem() - time)
                + " seconds"
        );
    }

    /// <summary>
    /// This will create the TileMap based on the atlas data generated. Must be used after GenerateMap().
    /// </summary>
    /// <param name="tilemap">The tilemap you want to set the tiles on.</param>
    public void GenerateTilemap(TileMap tilemap)
    {
        for (int z = 0; z < TileAtlasMap.Length; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    if (tilemap.GetLayersCount() - 1 < z)
                    {
                        GD.PrintErr(
                            "[Threaded Autotiler] Your tilemap has no layer "
                                + z
                                + "! Temporarily creating it, but you should add it in the editor for more control."
                        );
                        tilemap.AddLayer(z);
                    }
                    tilemap.SetCell(z, new Vector2I(x, y), 0, TileAtlasMap[z][x, y]);
                }
            }
        }
    }

    private void LoadTerrainData()
    {
        PluginSaveHandler.LoadData(out _tileData, out _terrainData);
        TerrainMap = new bool[_terrainData.Count][,];
        BitmaskMap = new ushort[_terrainData.Count][,];
        TileAtlasMap = new Vector2I[_terrainData.Count][,];

        CreateBitmaskTiles();
    }

    private void GenerateTerrainMap(FastNoiseLite noise)
    {
        int count = 0;
        foreach (TerrainData td in _terrainData)
        {
            bool[,] terrains = new bool[mapSize, mapSize];
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    float value = noise.GetNoise2D(x, y);
                    if (value > td.Height)
                    {
                        terrains[x, y] = true;
                    }
                    else
                    {
                        terrains[x, y] = false;
                    }
                }
            }
            TerrainMap[count] = terrains;
            count++;
        }
    }

    private void GenerateBitmaskMap()
    {
        int z = 0;
        foreach (bool[,] terrain in TerrainMap)
        {
            Stage++;
            ushort[,] bitmasks = new ushort[mapSize, mapSize];
            for (int x = 0; x < mapSize; x++)
            {
                StageProgress = (float)x / (float)mapSize * 100;
                for (int y = 0; y < mapSize; y++)
                {
                    if (!terrain[x, y])
                    {
                        bitmasks[x, y] = 0;
                        continue;
                    }
                    //Initialize the booleans for each direction
                    bool t,
                        b,
                        l,
                        r,
                        tl,
                        tr,
                        bl,
                        br;
                    t = b = l = r = tl = tr = bl = br = false;

                    //If we are at the map edges, use the UseEdges value to determine if we should include the edge tiles (for chunking)
                    if (x == 0)
                    {
                        l = useEdges;
                        tl = useEdges;
                        bl = useEdges;
                    }
                    if (x == mapSize - 1)
                    {
                        r = useEdges;
                        tr = useEdges;
                        br = useEdges;
                    }
                    if (y == 0)
                    {
                        t = useEdges;
                        tl = useEdges;
                        tr = useEdges;
                    }
                    if (y == mapSize - 1)
                    {
                        b = useEdges;
                        bl = useEdges;
                        br = useEdges;
                    }

                    //If we are not at the map edges, check the surrounding tiles to see if we should include them
                    if (x > 0)
                    {
                        l = terrain[x - 1, y];
                        if (y > 0)
                            tl = terrain[x - 1, y - 1];
                        if (y < mapSize - 1)
                            bl = terrain[x - 1, y + 1];
                    }
                    if (x < mapSize - 1)
                    {
                        r = terrain[x + 1, y];
                        if (y > 0)
                            tr = terrain[x + 1, y - 1];
                        if (y < mapSize - 1)
                            br = terrain[x + 1, y + 1];
                    }
                    if (y > 0)
                    {
                        t = terrain[x, y - 1];
                        if (x > 0)
                            tl = terrain[x - 1, y - 1];
                        if (x < mapSize - 1)
                            tr = terrain[x + 1, y - 1];
                    }
                    if (y < mapSize - 1)
                    {
                        b = terrain[x, y + 1];
                        if (x > 0)
                            bl = terrain[x - 1, y + 1];
                        if (x < mapSize - 1)
                            br = terrain[x + 1, y + 1];
                    }

                    //Convert the booleans to a bitmask
                    bitmasks[x, y] = BitmaskConverter.ConvertTileToBitmask(
                        t,
                        b,
                        l,
                        r,
                        tl,
                        tr,
                        bl,
                        br
                    );
                }
            }
            BitmaskMap[z] = bitmasks;
            z++;
        }
    }

    private void GenerateAtlasMap()
    {
        int z = 0;
        foreach (ushort[,] bitmask in BitmaskMap)
        {
            Vector2I[,] atlas = new Vector2I[mapSize, mapSize];
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    if (!TerrainMap[z][x, y])
                    {
                        atlas[x, y] = new Vector2I(-1, -1);
                        continue;
                    }
                    atlas[x, y] = GetAtlasTile(_terrainData[z].Name, bitmask[x, y]);
                }
            }
            TileAtlasMap[z] = atlas;
            z++;
        }
    }

    private void CreateBitmaskTiles()
    {
        foreach (TerrainData terrain in _terrainData)
        {
            List<TileData> tileData = _tileData[terrain.Name];
            List<BitmaskTile> tiles = new List<BitmaskTile>();
            foreach (TileData tile in tileData)
            {
                tiles.Add(
                    new BitmaskTile(tile.AtlasCoords, GetBitmaskValueFromStringName(tile.TileMode))
                );
            }
            _bitmaskTiles.Add(terrain.Name, tiles.ToArray());
        }
    }

    private ushort GetBitmaskValueFromStringName(string name)
    {
        switch (name)
        {
            case "Top":
                return BitmaskStatic.Bottom;
            case "Bottom":
                return BitmaskStatic.Top;
            case "Left":
                return BitmaskStatic.Right;
            case "Right":
                return BitmaskStatic.Left;
            case "TR":
                return BitmaskStatic.BottomLeft;
            case "TL":
                return BitmaskStatic.BottomRight;
            case "BR":
                return BitmaskStatic.TopLeft;
            case "BL":
                return BitmaskStatic.TopRight;
            case "LeftRight":
                return BitmaskStatic.SingleLeftRight;
            case "UpDown":
                return BitmaskStatic.SingleTopBottom;
            case "SingleLeft":
                return BitmaskStatic.SingleLeft;
            case "SingleRight":
                return BitmaskStatic.SingleRight;
            case "SingleBottom":
            case "SingleDown":
                return BitmaskStatic.SingleBottom;
            case "SingleTop":
                return BitmaskStatic.SingleTop;
            case "Single":
                return BitmaskStatic.Single;
            case "Center":
            default:
                return 255;
        }
    }

    private Vector2I GetAtlasTile(string terrainName, ushort bitmask)
    {
        foreach (BitmaskTile tile in _bitmaskTiles[terrainName])
        {
            if (tile.BitmaskValue == bitmask)
            {
                return tile.AtlasValue;
            }
        }
        if (bitmask > 0)
        {
            foreach (TileData tile in _tileData[terrainName])
            {
                if (tile.TileMode == "Center")
                {
                    return tile.AtlasCoords;
                }
            }
            if (_tileData[terrainName].Count > 0)
            {
                return _tileData[terrainName][0].AtlasCoords;
            }
        }

        return new Vector2I(-1, -1);
    }

    public int GetMaxStage()
    {
        return _terrainData.Count;
    }
}
