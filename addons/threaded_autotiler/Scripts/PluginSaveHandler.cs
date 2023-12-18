using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public class PluginSaveHandler
{
    private const string SaveFileName = "res://addons/threaded_autotiler/EditorData/Terrains";

    public static void SaveData(Dictionary<string, List<TileData>> data, List<TerrainData> terrains)
    {
        using FileAccess file = FileAccess.Open(SaveFileName, FileAccess.ModeFlags.Write);

        GD.Print("Saving data");

        List<TerrainData> sortedList = terrains.OrderBy(o => o.Layer).ToList();
        file.StoreVar(data.Count);

        foreach (TerrainData td in sortedList)
        {
            file.StoreVar(td.Name);
            file.StoreVar(td.Color.ToHtml());
            file.StoreVar(td.Biome);
            file.StoreVar(td.Height);
            file.StoreVar(td.Layer);
            file.StoreVar(data[td.Name].Count);
            foreach (TileData tile in data[td.Name])
            {
                file.StoreVar(tile.Id);
                file.StoreVar(tile.AtlasCoords.X);
                file.StoreVar(tile.AtlasCoords.Y);
                file.StoreVar(tile.TileMode);
            }
        }
        file.Close();
        GD.Print("Done saving data");
    }

    public static void LoadData(
        out Dictionary<string, List<TileData>> tileData,
        out List<TerrainData> terrainData
    )
    {
        GD.Print("Loading data");
        using FileAccess file = FileAccess.Open(SaveFileName, FileAccess.ModeFlags.Read);

        terrainData = new List<TerrainData>();
        tileData = new Dictionary<string, List<TileData>>();

        if (file == null || file.GetLength() == 0)
        {
            return;
        }

        int layerCount = (int)file.GetVar();
        for (int i = 0; i < layerCount; i++)
        {
            string layer = (string)file.GetVar();
            string color = (string)file.GetVar();
            float biome = (float)file.GetVar();
            float height = (float)file.GetVar();
            int layerIndex = (int)file.GetVar();
            TerrainData td = new TerrainData(layer, new Color(color), biome, height, layerIndex);
            terrainData.Add(td);
            int tileCount = (int)file.GetVar();
            List<TileData> tiles = new List<TileData>();

            for (int j = 0; j < tileCount; j++)
            {
                int id = (int)file.GetVar();
                int atlasCoordsX = (int)file.GetVar();
                int atlasCoordsY = (int)file.GetVar();
                string tileMode = (string)file.GetVar();

                TileData tile = new TileData(
                    id,
                    new Vector2I(atlasCoordsX, atlasCoordsY),
                    tileMode
                );
                tiles.Add(tile);
            }

            tileData[layer] = tiles;
        }

        GD.Print("Done loading data");
    }
}
