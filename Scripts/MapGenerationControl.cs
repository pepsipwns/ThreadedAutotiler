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
    bool GenerateOnThread = true;

    [Export]
    private int mapSize = 100;

    [Export]
    private bool useEdges = false;

    private Thread thread;

    public void OnGeneratePressed()
    {
        if (GenerateOnThread)
        {
            if (thread != null && thread.IsAlive)
            {
                return;
            }
            thread = new Thread(
                () =>
                    MapGeneration.Instance.GenerateMap(
                        NoiseHandler.Instance.Noise,
                        mapSize,
                        useEdges
                    )
            );
            thread.Start();
        }
        else
        {
            MapGeneration.Instance.GenerateMap(NoiseHandler.Instance.Noise, mapSize, useEdges);
            MapGeneration.Instance.GenerateTilemap(Tilemap);
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
            MapGeneration.Instance.GenerateTilemap(Tilemap);
        }
    }
}
