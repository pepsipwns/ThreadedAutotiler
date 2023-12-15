using Godot;
using System;
using System.Threading;

public partial class MapGenerationControl : Node2D
{
    [Export]
    Label LoadingLabel;

    [Export]
    TileMap Tilemap;

    [Export]
    bool GenerateOnThread = false;

    private Thread thread;

    private Vector2I[][,] _atlasMap;

    public void OnGeneratePressed()
    {
        if (GenerateOnThread)
        {
            if (thread != null && thread.IsAlive)
            {
                return;
            }
            thread = new Thread(() => MapGeneration.Instance.GenerateMap());
            thread.Start();
        }
        else
        {
            MapGeneration.Instance.GenerateMap();
            GenerateTilemap();
        }
    }

    public override void _Process(double delta)
    {
        if (thread != null && thread.IsAlive)
        {
            int stage = MapGeneration.Instance.Stage;
            int maxStage = MapGeneration.Instance.GetMaxStage();
            float progress = MapGeneration.Instance.StageProgress;
            LoadingLabel.Text =
                "Loading... Stage " + stage + "/" + maxStage + " Progress: " + progress + "%";
        }
        if (thread != null && thread.ThreadState == ThreadState.Stopped)
        {
            LoadingLabel.Text = "Done!";
            thread = null;
            GenerateTilemap();
        }
    }

    private void GenerateTilemap()
    {
        _atlasMap = MapGeneration.Instance.TileAtlasMap;
        for (int z = 0; z < _atlasMap.Length; z++)
        {
            for (int x = 0; x < MapGeneration.Instance.MapSize; x++)
            {
                for (int y = 0; y < MapGeneration.Instance.MapSize; y++)
                {
                    Tilemap.SetCell(z, new Vector2I(x, y), 0, _atlasMap[z][x, y]);
                }
            }
        }
    }
}
