using System;
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
    [Export]
    public int MapSize = 100;

    [Export]
    public bool UseEdges = false;
    public static MapGeneration Instance { get; private set; }
    public bool MapGenerated = false;
    public int Stage { get; private set; }
    public float StageProgress { get; private set; }
    public bool[][,] TerrainMap = new bool[TerrainHandler.TerrainTypes.Length][,];
    public ushort[][,] BitmaskMap = new ushort[TerrainHandler.TerrainTypes.Length][,];
    public Vector2I[][,] TileAtlasMap = new Vector2I[TerrainHandler.TerrainTypes.Length][,];

    public override void _Ready()
    {
        Instance = this;
    }

    public void GenerateMap()
    {
        double time = Time.GetUnixTimeFromSystem();
        GenerateTerrainMap();
        GenerateBitmaskMap();
        GenerateAtlasMap();
        MapGenerated = true;
        GD.Print("Map generated in " + (Time.GetUnixTimeFromSystem() - time) + " seconds");
    }

    private void GenerateTerrainMap()
    {
        foreach (TerrainType z in TerrainHandler.TerrainTypes)
        {
            bool[,] terrains = new bool[MapSize, MapSize];
            for (int x = 0; x < MapSize; x++)
            {
                for (int y = 0; y < MapSize; y++)
                {
                    float noise = NoiseHandler.Instance.GetNoise(x, y);
                    if (noise > TerrainHandler.GetTerrainHeight(z))
                    {
                        terrains[x, y] = true;
                    }
                    else
                    {
                        terrains[x, y] = false;
                    }
                }
            }
            TerrainMap[(int)z] = terrains;
        }
    }

    private void GenerateBitmaskMap()
    {
        int z = 0;
        foreach (bool[,] terrain in TerrainMap)
        {
            Stage++;
            ushort[,] bitmasks = new ushort[MapSize, MapSize];
            for (int x = 0; x < MapSize; x++)
            {
                StageProgress = (float)x / (float)MapSize;
                for (int y = 0; y < MapSize; y++)
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
                        l = UseEdges;
                        tl = UseEdges;
                        bl = UseEdges;
                    }
                    if (x == MapSize - 1)
                    {
                        r = UseEdges;
                        tr = UseEdges;
                        br = UseEdges;
                    }
                    if (y == 0)
                    {
                        t = UseEdges;
                        tl = UseEdges;
                        tr = UseEdges;
                    }
                    if (y == MapSize - 1)
                    {
                        b = UseEdges;
                        bl = UseEdges;
                        br = UseEdges;
                    }

                    //If we are not at the map edges, check the surrounding tiles to see if we should include them
                    if (x > 0)
                    {
                        l = terrain[x - 1, y];
                        if (y > 0)
                            tl = terrain[x - 1, y - 1];
                        if (y < MapSize - 1)
                            bl = terrain[x - 1, y + 1];
                    }
                    if (x < MapSize - 1)
                    {
                        r = terrain[x + 1, y];
                        if (y > 0)
                            tr = terrain[x + 1, y - 1];
                        if (y < MapSize - 1)
                            br = terrain[x + 1, y + 1];
                    }
                    if (y > 0)
                    {
                        t = terrain[x, y - 1];
                        if (x > 0)
                            tl = terrain[x - 1, y - 1];
                        if (x < MapSize - 1)
                            tr = terrain[x + 1, y - 1];
                    }
                    if (y < MapSize - 1)
                    {
                        b = terrain[x, y + 1];
                        if (x > 0)
                            bl = terrain[x - 1, y + 1];
                        if (x < MapSize - 1)
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
            Vector2I[,] atlas = new Vector2I[MapSize, MapSize];
            for (int x = 0; x < MapSize; x++)
            {
                for (int y = 0; y < MapSize; y++)
                {
                    atlas[x, y] = TerrainHandler.Instance.GetAtlasTile(
                        (TerrainType)z,
                        bitmask[x, y],
                        x,
                        y
                    );
                }
            }
            TileAtlasMap[z] = atlas;
            z++;
        }
    }

    public int GetMaxStage()
    {
        return TerrainMap.Length;
    }
}
