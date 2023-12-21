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
        List<TerrainData> terrains
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
                }
            }
        }
        file.Close();
    }

    public static void LoadData(
        out Dictionary<string, List<List<TileData>>> tileData,
        out List<TerrainData> terrainData
    )
    {
        using FileAccess file = FileAccess.Open(SaveFileName, FileAccess.ModeFlags.Read);

        terrainData = new List<TerrainData>();
        tileData = new Dictionary<string, List<List<TileData>>>();

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
                    tileVariants.Add(
                        new TileData(id, new Vector2I(atlasX, atlasY), tileMode, chance)
                    );
                }
                tiles.Add(tileVariants);
            }
            tileData[name] = tiles;
        }
    }
}
