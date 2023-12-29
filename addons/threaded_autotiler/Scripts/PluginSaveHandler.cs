using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public class PluginSaveHandler
{
    private const string SaveFileName = "res://addons/threaded_autotiler/EditorData/Terrains";

    public static void SaveData(
        Dictionary<string, List<List<TileData>>> data,
        List<TerrainData> terrains,
        Dictionary<string, List<CustomBitmaskData>> _customBitmaskData
    )
    {
        using FileAccess file = FileAccess.Open(SaveFileName, FileAccess.ModeFlags.Write);

        List<TerrainData> sortedList = terrains.OrderBy(o => o.Layer).ToList();
        file.StoreVar(data.Count); // Terrain Count

        foreach (TerrainData td in sortedList)
        {
            file.StoreVar(td.Name); // Terrain Name
            file.StoreVar(td.Color.ToHtml()); // Terrain Color
            file.StoreVar(td.Biome); // Terrain Biome
            file.StoreVar(td.Height); // Terrain Height
            file.StoreVar(td.Layer); // Terrain Layer
            file.StoreVar(data[td.Name].Count); // Tile Count
            foreach (List<TileData> tiles in data[td.Name])
            {
                file.StoreVar(tiles.Count); // Tile Variant Count
                foreach (TileData tileVariant in tiles)
                {
                    file.StoreVar(tileVariant.Id); // Tile Id
                    file.StoreVar(tileVariant.AtlasCoords.X); // Tile Atlas X
                    file.StoreVar(tileVariant.AtlasCoords.Y); // Tile Atlas Y
                    file.StoreVar(tileVariant.TileMode); // Tile Mode
                    file.StoreVar(tileVariant.Chance); // Tile Chance
                    file.StoreVar(tileVariant.DecorativeTiles.Count); // Decorative Tile Count
                    foreach (DecorativeTileData decorativeTile in tileVariant.DecorativeTiles)
                    {
                        file.StoreVar(decorativeTile.AtlasCoords.X); // Decorative Tile Atlas X
                        file.StoreVar(decorativeTile.AtlasCoords.Y); // Decorative Tile Atlas Y
                        file.StoreVar(decorativeTile.Direction); // Decorative Direction
                        file.StoreVar(decorativeTile.Chance); // Decorative Tile Chance
                    }
                    file.StoreVar(tileVariant.TileBitmasks.Length); // Tile Bitmask Length
                    foreach (bool bitmask in tileVariant.TileBitmasks)
                    {
                        file.StoreVar(bitmask); // Tile Bitmask
                    }
                }
            }
            file.StoreVar(_customBitmaskData.ContainsKey(td.Name));
            if (_customBitmaskData.ContainsKey(td.Name))
            {
                file.StoreVar(_customBitmaskData[td.Name].Count); // Custom Bitmask Count
                foreach (List<CustomBitmaskData> customBitmaskData in _customBitmaskData.Values)
                {
                    foreach (CustomBitmaskData cbd in customBitmaskData)
                    {
                        file.StoreVar(cbd.Name); // Custom Bitmask Name
                        file.StoreVar(cbd.Bitmasks.Length); // Custom Bitmask Length
                        foreach (bool bitmask in cbd.Bitmasks)
                        {
                            file.StoreVar(bitmask); // Custom Bitmask
                        }
                    }
                }
            }
        }
        file.Close();
    }

    public static void LoadData(
        out Dictionary<string, List<List<TileData>>> tileData,
        out List<TerrainData> terrainData,
        out Dictionary<string, List<CustomBitmaskData>> _customBitmaskData
    )
    {
        using FileAccess file = FileAccess.Open(SaveFileName, FileAccess.ModeFlags.Read);

        terrainData = new List<TerrainData>();
        tileData = new Dictionary<string, List<List<TileData>>>();
        _customBitmaskData = new Dictionary<string, List<CustomBitmaskData>>();

        if (file == null || file.GetLength() == 0)
        {
            return;
        }

        int layerCount = (int)file.GetVar(); // Terrain Count
        for (int i = 0; i < layerCount; i++)
        {
            string name = (string)file.GetVar(); // Terrain Name
            string color = (string)file.GetVar(); // Terrain Color
            float biome = (float)file.GetVar(); // Terrain Biome
            float height = (float)file.GetVar(); // Terrain Height
            int layer = (int)file.GetVar(); // Terrain Layer
            TerrainData td = new TerrainData(name, new Color(color), biome, height, layer);
            terrainData.Add(td);

            int tileCount = (int)file.GetVar(); // Tile Count
            List<List<TileData>> tiles = new List<List<TileData>>();
            for (int x = 0; x < tileCount; x++)
            {
                int tileVariantCount = (int)file.GetVar(); // Tile Variant Count
                List<TileData> tileVariants = new List<TileData>();
                for (int y = 0; y < tileVariantCount; y++)
                {
                    int id = (int)file.GetVar(); // Tile Id
                    int atlasX = (int)file.GetVar(); // Tile Atlas X
                    int atlasY = (int)file.GetVar(); // Tile Atlas Y
                    string tileMode = (string)file.GetVar(); // Tile Mode
                    float chance = (float)file.GetVar(); // Tile Chance
                    int decorativeTileCount = (int)file.GetVar(); // Decorative Tile Count
                    List<DecorativeTileData> decorativeTiles = new List<DecorativeTileData>();
                    for (int z = 0; z < decorativeTileCount; z++)
                    {
                        int decorativeAtlasX = (int)file.GetVar(); // Decorative Tile Atlas X
                        int decorativeAtlasY = (int)file.GetVar(); // Decorative Tile Atlas Y
                        int direction = (int)file.GetVar(); // Decorative Direction
                        float decorativeChance = (float)file.GetVar(); // Decorative Tile Chance
                        decorativeTiles.Add(
                            new DecorativeTileData(
                                new Vector2I(decorativeAtlasX, decorativeAtlasY),
                                direction,
                                decorativeChance
                            )
                        );
                    }

                    int bitmaskLength = (int)file.GetVar(); // Tile Bitmask Length
                    bool[] bitmasks = new bool[bitmaskLength];
                    for (int z = 0; z < bitmaskLength; z++)
                    {
                        bitmasks[z] = (bool)file.GetVar(); // Tile Bitmask
                    }
                    tileVariants.Add(
                        new TileData(
                            id,
                            new Vector2I(atlasX, atlasY),
                            tileMode,
                            bitmasks,
                            chance,
                            decorativeTiles
                        )
                    );
                }
                tiles.Add(tileVariants);
            }
            tileData[name] = tiles;

            bool hasCustomBitmask = (bool)file.GetVar();
            if (hasCustomBitmask)
            {
                int customBitmaskCount = (int)file.GetVar(); // Custom Bitmask Count
                List<CustomBitmaskData> customBitmaskData = new List<CustomBitmaskData>();
                for (int x = 0; x < customBitmaskCount; x++)
                {
                    string customBitmaskName = (string)file.GetVar(); // Custom Bitmask Name
                    int customBitmaskLength = (int)file.GetVar(); // Custom Bitmask Length
                    bool[] customBitmask = new bool[customBitmaskLength];
                    for (int y = 0; y < customBitmaskLength; y++)
                    {
                        customBitmask[y] = (bool)file.GetVar(); // Custom Bitmask
                    }
                    customBitmaskData.Add(new CustomBitmaskData(customBitmaskName, customBitmask));
                }
                _customBitmaskData[name] = customBitmaskData;
            }
        }
    }
}
