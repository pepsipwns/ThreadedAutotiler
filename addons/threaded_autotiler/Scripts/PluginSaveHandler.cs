using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class PluginSaveHandler
{
    private const string SaveFileName = "res://addons/threaded_autotiler/EditorData/Terrains";

    public static void SaveData(Dictionary<string, List<TileData>> data, List<TerrainData> terrains)
    {
        using FileAccess file = FileAccess.Open(SaveFileName, FileAccess.ModeFlags.Write);

        GD.Print("Saving data");

        file.StoreVar(data.Count);
        foreach (string layer in data.Keys)
        {
            TerrainData td = terrains.Find(t => t.Name == layer);
            file.StoreVar(layer);
            file.StoreVar(td.Color.ToHtml());
            file.StoreVar(data[layer].Count);
            foreach (TileData tile in data[layer])
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
            TerrainData td = new TerrainData(layer, new Color((string)file.GetVar()));
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
